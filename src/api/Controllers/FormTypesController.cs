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
    public class FormTypesController : ODataController
    {
        private readonly FormTypeRepository _formTypeContext;

        public FormTypesController(FormTypeRepository formTypeContext)
        {
            _formTypeContext = formTypeContext;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "GetFormTypes")]
        public async Task<ActionResult<IEnumerable<FormTypeDto>>> GetFormTypes()
        {
            var res = await _formTypeContext.GetFormTypes();
            List<FormTypeDto> list = new List<FormTypeDto>();

            foreach (var item in res)
            {
                list.Add(
                    new FormTypeDto
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Description = item.Description,
                        CustomerId = item.CustomerId
                    }
                );
            }
            return list;
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "GetFormType")]
        public async Task<ActionResult<FormTypeDto>> GetFormType(Guid Id) 
        {
            var res = await _formTypeContext.GetFormType(Id);
            if(res == null) 
            {
                return NotFound();
            }

            return new FormTypeDto {
                Id = res.Id,
                Code = res.Code,
                Description = res.Description,
                CustomerId = res.CustomerId
            };
        }

        [HttpPost]
        [Authorize(Roles = "CreateFormType")]
        public async Task<ActionResult<FormTypeDto>> CreateFormType(FormTypeDto FormTypedto)
        {
            try
            {
                var FormType = new FormType 
                {
                    Id = FormTypedto.Id,
                    Code = FormTypedto.Code,
                    Description = FormTypedto.Description,
                    CustomerId = FormTypedto.CustomerId
                };

                await _formTypeContext.CreateFormType(FormType);
                return FormTypedto;
            }
            catch(Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "UpdateFormType")]
        public async Task<ActionResult<FormTypeDto>> UpdateFormType(Guid Id, FormTypeDto FormTypedto)
        {
            if(Id != FormTypedto.Id) 
            {
                return BadRequest();
            }

            var tempFormType = await _formTypeContext.GetFormType(Id);
            if(tempFormType == null) 
            {
                return NotFound();
            }

            try 
            {
                tempFormType.Code = FormTypedto.Code;
                tempFormType.Description = FormTypedto.Description;
                tempFormType.CustomerId = FormTypedto.CustomerId;
                await _formTypeContext.UpdateFormType(tempFormType);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
            return NoContent();

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeleteFormType")]
        public async Task<ActionResult> DeleteFormType(Guid Id)
        {
            var tempFormType = await _formTypeContext.GetFormType(Id);
            if(tempFormType == null) 
            {
                return NotFound();
            }

            try{
                await _formTypeContext.DeleteFormType(tempFormType);
                return NoContent();
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }
    }
}