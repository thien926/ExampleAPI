using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : ODataController
    {
        private readonly RoleRepository _roleRepository;
        private readonly RolePermissionRepository _rolePermissionRepository;
        public readonly PermissionRepository _permissionRepository;
        
        
        

        public RolesController(RoleRepository roleRepository, RolePermissionRepository rolePermissionRepository,
        PermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "GetRoles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var res = await _roleRepository.GetRoles();
            List<RoleDto> list = new List<RoleDto>();

            foreach (var item in res)
            {
                list.Add(
                    new RoleDto
                    {
                        Id = item.Id,
                        Name = item.Name
                    }
                );
            }
            return list;
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "GetRole")]
        public async Task<ActionResult<RoleDto>> GetRole(Guid Id) 
        {
            var res = await _roleRepository.GetRole(Id);
            if(res == null) 
            {
                return NotFound();
            }

            IEnumerable<RolePermission> rolePermissions = await _rolePermissionRepository.GetRolePermissionsWithRoleId(Id);
            List<Guid> permissionIds = new List<Guid>();
            foreach (var item in rolePermissions)
            {
                permissionIds.Add(item.PermissionId);
            }
            IEnumerable<Permission> permissions = await _permissionRepository.GetPermissionsWithPermissionIds(permissionIds);
            
            var tempstr = "";
            foreach (var item in permissions)
            {
                tempstr += item.Id + " " + item.Name + "&";
            }

            return new RoleDto {
                Id = res.Id,
                Name = res.Name,
                Detail = tempstr
            };
        }

        [HttpPost]
        [Authorize(Roles = "CreateRole")]
        public async Task<ActionResult<RoleDto>> CreateRole(RoleDto Roledto)
        {
            try
            {
                var role = new Role 
                {
                    Id = Roledto.Id,
                    Name = Roledto.Name
                };
                role = await _roleRepository.CreateRole(role);
                Roledto.Id = role.Id;
                List<RolePermission> list = new List<RolePermission>();

                if(Roledto.Detail != null)
                {
                    string[] strarr = Roledto.Detail.Split('&');

                    foreach (var item in strarr)
                    {
                        list.Add(
                            new RolePermission {
                                RoleId = role.Id,
                                PermissionId = Guid.Parse(item)
                            }
                        );
                    }
                    await _rolePermissionRepository.CreateRolePermissions(list);
                }

                return Roledto;
            }
            catch(Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateRole")]
        public async Task<ActionResult<RoleDto>> UpdateRole(Guid Id, RoleDto roledto)
        {
            if(Id != roledto.Id) 
            {
                return BadRequest();
            }

            var temprole = await _roleRepository.GetRole(Id);
            if(temprole == null) 
            {
                return NotFound();
            }

            try 
            {
                temprole.Name = roledto.Name;
                await _roleRepository.UpdateRole(temprole);


                if(roledto.Detail != null)
                {
                    await _rolePermissionRepository.DeleteRolePermissionsWithRoleId(temprole.Id);
                    List<RolePermission> list = new List<RolePermission>();

                    string[] strarr = roledto.Detail.Split('&');
                    foreach (var item in strarr)
                    {
                        list.Add(
                            new RolePermission {
                                RoleId = temprole.Id,
                                PermissionId = Guid.Parse(item)
                            }
                        );
                    }
                    await _rolePermissionRepository.CreateRolePermissions(list);
                }
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeleteRole")]
        public async Task<ActionResult> DeleteRole(Guid Id)
        {
            var temprole = await _roleRepository.GetRole(Id);
            if(temprole == null) 
            {
                return NotFound();
            }

            try{
                await _roleRepository.DeleteRole(temprole);
                return NoContent();
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }
    }
}