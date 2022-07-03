using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence
{
    public class RoleRepository
    {
        private readonly RentContext _context;
        public RoleRepository(RentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRole(Guid Id) 
        {
            return await _context.Roles.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<Role> CreateRole(Role Role)
        {
            var res = _context.Roles.Add(Role);
            await _context.SaveChangesAsync();
            return Role;
        }

        public async Task<Role> UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> DeleteRole(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}