using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence
{
    public class TenantRepository
    {
        private readonly RentContext _context;
        public TenantRepository(RentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tenant>> GetTenants()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant?> GetTenant(Guid Id) 
        {
            return await _context.Tenants.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<Tenant> CreateTenant(Tenant Tenant)
        {
            _context.Tenants.Add(Tenant);
            await _context.SaveChangesAsync();
            return Tenant;
        }

        public async Task<Tenant> UpdateTenant(Tenant Tenant)
        {
            _context.Tenants.Update(Tenant);
            await _context.SaveChangesAsync();
            return Tenant;
        }

        public async Task<Tenant> DeleteTenant(Tenant Tenant)
        {
            _context.Tenants.Remove(Tenant);
            await _context.SaveChangesAsync();
            return Tenant;
        }
    }
}