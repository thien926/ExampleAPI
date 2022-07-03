using System.Threading.Tasks.Dataflow;
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
    public class CustomersController : ODataController
    {
        private readonly CustomerRepository _customerContext;
        private readonly HttpContextAccessor _httpContext;

        public CustomersController(CustomerRepository customerContext, HttpContextAccessor httpContext)
        {
            _customerContext = customerContext;
            _httpContext = httpContext;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "GetCustomers")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var Id = _httpContext.HttpContext?.User.FindFirst("Id")?.Value;
            Console.WriteLine(Id);

            var res = await _customerContext.GetCustomers();
            List<CustomerDto> list = new List<CustomerDto>();

            foreach (var item in res)
            {
                list.Add(
                    new CustomerDto
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        Phone = item.Phone,
                        MobilePhone = item.MobilePhone,
                        Gender = item.Gender,
                        Properties = item.Properties,
                        TenantId = item.TenantId
                    }
                );
            }
            return list;
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "GetCustomer")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(Guid Id) 
        {
            
            var res = await _customerContext.GetCustomer(Id);
            if(res == null) 
            {
                return NotFound();
            }

            return new CustomerDto {
                Id = res.Id,
                FirstName = res.FirstName,
                LastName = res.LastName,
                Email = res.Email,
                Phone = res.Phone,
                MobilePhone = res.MobilePhone,
                Gender = res.Gender,
                Properties = res.Properties,
                TenantId = res.TenantId
            };
        }

        [HttpPost]
        [Authorize(Roles = "CreateCustomer")]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto Customerdto)
        {
            try
            {
                var Customer = new Customer 
                {
                    Id = Customerdto.Id,
                    FirstName = Customerdto.FirstName,
                    LastName = Customerdto.LastName,
                    Email = Customerdto.Email,
                    Phone = Customerdto.Phone,
                    MobilePhone = Customerdto.MobilePhone,
                    Gender = Customerdto.Gender,
                    Properties = Customerdto.Properties,
                    TenantId = Customerdto.TenantId
                };

                await _customerContext.CreateCustomer(Customer);
                return Customerdto;
            }
            catch(Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateCustomer")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(Guid Id, CustomerDto Customerdto)
        {

            if(Id != Customerdto.Id) 
            {
                return BadRequest();
            }

            var tempCustomer = await _customerContext.GetCustomer(Id);
            if(tempCustomer == null) 
            {
                return NotFound();
            }

            try 
            {
                tempCustomer.FirstName = Customerdto.FirstName;
                tempCustomer.LastName = Customerdto.LastName;
                tempCustomer.Email = Customerdto.Email;
                tempCustomer.Phone = Customerdto.Phone;
                tempCustomer.MobilePhone = Customerdto.MobilePhone;
                tempCustomer.Gender = Customerdto.Gender;
                tempCustomer.Properties = Customerdto.Properties;
                tempCustomer.TenantId = Customerdto.TenantId;

                await _customerContext.UpdateCustomer(tempCustomer);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeleteCustomer")]
        public async Task<ActionResult> DeleteCustomer(Guid Id)
        {
            var tempCustomer = await _customerContext.GetCustomer(Id);
            if(tempCustomer == null) 
            {
                return NotFound();
            }

            try{
                await _customerContext.DeleteCustomer(tempCustomer);
                return NoContent();
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }
        
        
    }
}