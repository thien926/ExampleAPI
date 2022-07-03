using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence
{
    public class FormTypeRepository
    {
        private readonly RentContext _context;
        public FormTypeRepository(RentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FormType>> GetFormTypes()
        {
            return await _context.FormTypes.ToListAsync();
        }

        public async Task<FormType?> GetFormType(Guid Id) 
        {
            return await _context.FormTypes.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<FormType> CreateFormType(FormType FormType)
        {
            _context.FormTypes.Add(FormType);
            await _context.SaveChangesAsync();
            return FormType;
        }

        public async Task<FormType> UpdateFormType(FormType FormType)
        {
            _context.FormTypes.Update(FormType);
            await _context.SaveChangesAsync();
            return FormType;
        }

        public async Task<FormType> DeleteFormType(FormType FormType)
        {
            _context.FormTypes.Remove(FormType);
            await _context.SaveChangesAsync();
            return FormType;
        }
    }
}