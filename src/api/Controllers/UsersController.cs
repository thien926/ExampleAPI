using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Entities;
using api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using api.Data;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ODataController
    {
        private readonly UserRepository _userContext;

        public UsersController(UserRepository userContext)
        {
            _userContext = userContext;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "GetUsers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var res = await _userContext.GetUsers();
            List<UserDto> list = new List<UserDto>();
            foreach (var item in res)
            {
                list.Add(
                    new UserDto{
                        Id = item.Id,
                        Email = item.Email,
                        Name = item.Name,
                        // Password = item.Password,
                        RoleId = item.RoleId,
                        TenantId = item.TenantId
                    }
                );
            }
            return list;
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "GetUser")]
        public async Task<ActionResult<UserDto>> GetUser(Guid Id)
        {
            var res = await _userContext.GetUser(Id);
            if(res == null)
            {
                return NotFound();
            }

            return new UserDto {
                Id = res.Id,
                Email = res.Email,
                Name = res.Name,
                // Password = res.Password,
                RoleId = res.RoleId,
                TenantId = res.TenantId
            };
        }

        [HttpPost]
        [Authorize(Roles = "CreateUser")]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userdto)
        {
            try
            {
                HashPassword.CreatePasswordHash(userdto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var user = new User 
                {
                    Id = userdto.Id,
                    Name = userdto.Name,
                    Email = userdto.Email,
                    // Password = userdto.Password,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    RoleId = userdto.RoleId,
                    TenantId = userdto.TenantId
                };

                await _userContext.CreateUser(user);
                return userdto;
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateUser")]
        public async Task<ActionResult<UserUpdateDto>> UpdateUser(Guid Id, UserUpdateDto userdto)
        {
            if(Id != userdto.Id)
            {
                return BadRequest();
            }
            var tempUser = await _userContext.GetUser(Id);

            if(tempUser == null)
            {
                return NotFound();
            }

            try 
            {
                // _userContext.Users.Update(user);
                tempUser.Name = userdto.Name;
                tempUser.Email = userdto.Email;
                // tempUser.Password = userdto.Password;
                tempUser.RoleId = userdto.RoleId;
                tempUser.TenantId = userdto.TenantId;
                await _userContext.UpdateUser(tempUser);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeleteUser")]
        public async Task<ActionResult> DeleteUser(Guid Id)
        {
            var tempUser = await _userContext.GetUser(Id);
            if(tempUser == null)
            {
                return NotFound();
            }

            try{
                await _userContext.DeleteUser(tempUser);
                return NoContent();
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
            
        }
    }
}