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
    public class TenantsController : ODataController
    {
        private readonly TenantRepository _tenantContext;

        public TenantsController(TenantRepository tenantContext)
        {
            _tenantContext = tenantContext;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "GetTenants")]
        public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenants()
        {
            var res = await _tenantContext.GetTenants();
            List<TenantDto> list = new List<TenantDto>();

            foreach (var item in res)
            {
                list.Add(
                    new TenantDto
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
        [Authorize(Roles = "GetTenant")]
        public async Task<ActionResult<TenantDto>> GetTenant(Guid Id) 
        {
            var res = await _tenantContext.GetTenant(Id);
            if(res == null) 
            {
                return NotFound();
            }

            return new TenantDto {
                Id = res.Id,
                Name = res.Name
            };
        }

        [HttpPost]
        [Authorize(Roles = "CreateTenant")]
        public async Task<ActionResult<TenantDto>> CreateTenant(TenantDto tenantdto)
        {
            try
            {
                var tenant = new Tenant 
                {
                    Id = tenantdto.Id,
                    Name = tenantdto.Name
                };

                await _tenantContext.CreateTenant(tenant);
                return tenantdto;
            }
            catch(Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateTenant")]
        public async Task<ActionResult<TenantDto>> UpdateTenant(Guid Id, TenantDto tenantdto)
        {
            if(Id != tenantdto.Id) 
            {
                return BadRequest();
            }

            var tempTenant = await _tenantContext.GetTenant(Id);
            if(tempTenant == null) 
            {
                return NotFound();
            }

            try 
            {
                tempTenant.Name = tenantdto.Name;
                await _tenantContext.UpdateTenant(tempTenant);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeleteTenant")]
        public async Task<ActionResult> DeleteTenant(Guid Id)
        {
            var tempTenant = await _tenantContext.GetTenant(Id);
            if(tempTenant == null) 
            {
                return NotFound();
            }

            try{
                await _tenantContext.DeleteTenant(tempTenant);
                return NoContent();
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }
    }
}