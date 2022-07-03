using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using api.Data;
using Microsoft.AspNetCore.Authorization;
using api.Entities;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ODataController
    {
        private readonly UserRepository _userContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RolePermissionRepository _rolePermissionRepository;
        private readonly PermissionRepository _permissionRepository;

        public AuthController(UserRepository userContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
        RolePermissionRepository rolePermissionRepository, PermissionRepository permissionRepository)
        {
            _userContext = userContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
        }

        [HttpPost("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto LoginUserDto)
        {
            var tempUser = await _userContext.GetUserByEmail(LoginUserDto.Email);

            if(tempUser == null)
            {
                return NotFound();
            }

            if(!HashPassword.VerifyPasswordHash(LoginUserDto.Password, tempUser.PasswordHash, tempUser.PasswordSalt))
            {
                return BadRequest();
            }

            IEnumerable<RolePermission> rolePermissions = await _rolePermissionRepository.GetRolePermissionsWithRoleId(tempUser.RoleId);
            List<Guid> permissionIds = new List<Guid>();
            foreach (var item in rolePermissions)
            {
                permissionIds.Add(item.PermissionId);
            }
            IEnumerable<Permission> permissions = await _permissionRepository.GetPermissionsWithPermissionIds(permissionIds);

            string token = Token.CreateToken(tempUser, _configuration.GetSection("AppSettings:SecretKey").Value, permissions);
            return Ok(token);
            // return NoContent();
        }

        [HttpGet("getuser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            // if(_httpContextAccessor.HttpContext == null) 
            // {
            //     return Unauthorized();
            // }
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst("Id")?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userContext.GetUser(Guid.Parse(userId));
            if(user == null)
            {
                return NotFound();
            }

            // IEnumerable<RolePermission> rolePermissions = await _rolePermissionRepository.GetRolePermissionsWithRoleId(user.RoleId);
            // List<Guid> permissionIds = new List<Guid>();
            // foreach (var item in rolePermissions)
            // {
            //     permissionIds.Add(item.PermissionId);
            // }
            // IEnumerable<Permission> permissions = await _permissionRepository.GetPermissionsWithPermissionIds(permissionIds);
            // var tempstr = "";
            // foreach (var item in permissions)
            // {
            //     tempstr += item.Id + " " + item.Name + "&";
            // }

            // UserDto userdto = new UserDto {
            //     Id = user.Id,
            //     Email = user.Email,
            //     Name = user.Name,
            //     RoleId = user.RoleId,
            //     TenantId = user.TenantId
            // };

            // return new CurrentUser {
            //     user = userdto,
            //     permissions = tempstr
            // };

            return new UserDto {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                RoleId = user.RoleId,
                TenantId = user.TenantId
            };
        }
    }
}