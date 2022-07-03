
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Entities;
using api.Persistence;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using api.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PermissionsController : ODataController
    {

        private readonly PermissionRepository _permissionContext;

        public PermissionsController(PermissionRepository permissionContext)
        {
            _permissionContext = permissionContext;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "GetPermissions")]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
        {
            var res = await _permissionContext.GetPermissions();
            List<PermissionDto> list = new List<PermissionDto>();

            foreach (var item in res)
            {
                list.Add(
                    new PermissionDto
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
        [Authorize(Roles = "GetPermission")]
        public async Task<ActionResult<PermissionDto>> GetPermission(Guid Id) 
        {
            var res = await _permissionContext.GetPermission(Id);
            if(res == null) 
            {
                return NotFound();
            }

            return new PermissionDto {
                Id = res.Id,
                Name = res.Name
            };
        }

        [HttpPost]
        [Authorize(Roles = "CreatePermission")]
        public async Task<ActionResult<PermissionDto>> CreatePermission(PermissionDto Permissiondto)
        {
            try
            {
                var Permission = new Permission 
                {
                    Id = Permissiondto.Id,
                    Name = Permissiondto.Name
                };

                await _permissionContext.CreatePermission(Permission);
                return Permissiondto;
            }
            catch(Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdatePermission")]
        public async Task<ActionResult<PermissionDto>> UpdatePermission(Guid Id, PermissionDto Permissiondto)
        {
            if(Id != Permissiondto.Id) 
            {
                return BadRequest();
            }
            
            var tempPermission = await _permissionContext.GetPermission(Id);
            if(tempPermission == null) 
            {
                return NotFound();
            }
            
            try 
            {
                tempPermission.Name = Permissiondto.Name;
                await _permissionContext.UpdatePermission(tempPermission);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeletePermission")]
        public async Task<ActionResult> DeletePermission(Guid Id)
        {
            var tempPermission = await _permissionContext.GetPermission(Id);
            if(tempPermission == null) 
            {
                return NotFound();
            }

            try{
                await _permissionContext.DeletePermission(tempPermission);
                return NoContent();
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }
    }
}
