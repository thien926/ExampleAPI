using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence
{
    public class RolePermissionRepository
    {
        private readonly RentContext _context;
        public RolePermissionRepository(RentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissions()
        {
            return await _context.RolePermissions.ToListAsync();
        }

        // public async Task<RolePermission?> GetRolePermission(Guid Id) 
        // {
        //     return await _context.RolePermissions.FirstOrDefaultAsync(m => m.Id == Id);
        // }

        public async Task<RolePermission> CreateRolePermission(RolePermission RolePermission)
        {
            var res = _context.RolePermissions.Add(RolePermission);
            await _context.SaveChangesAsync();
            return RolePermission;
        }

        public async Task<IEnumerable<RolePermission>> CreateRolePermissions(IEnumerable<RolePermission> list)
        {
            var res = _context.RolePermissions.AddRangeAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task<RolePermission> UpdateRolePermission(RolePermission rolePermission)
        {
            _context.RolePermissions.Update(rolePermission);
            await _context.SaveChangesAsync();
            return rolePermission;
        }

        public async Task<RolePermission> DeleteRolePermission(RolePermission rolePermission)
        {
            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
            return rolePermission;
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissionsWithRoleId(Guid RoleId)
        {
            var query = _context.RolePermissions.AsQueryable();
            query = query.Where(m => m.RoleId == RoleId);
            return await query.ToListAsync();
        }

        public bool CheckRoleIdAndPermissionId(Guid RoleId, Guid PermissionId)
        {
            var query = _context.RolePermissions.AsQueryable();
            query = query.Where(m => m.RoleId == RoleId && m.PermissionId == PermissionId);
            return query.Count() > 0;
        }

        // public async Task<IEnumerable<Guid>> GetPermissionIdsWithRoleId(Guid RoleId)
        // {
        //     var query = _context.RolePermissions.AsQueryable();
        //     query = query.Where(m => m.RoleId == RoleId).Include(m => m.PermissionId);
        //     return (IEnumerable<Guid>)await query.ToListAsync();

        // }

        public async Task<IEnumerable<RolePermission>> DeleteRolePermissionsWithRoleId(Guid RoleId)
        {
            var list = await GetRolePermissionsWithRoleId(RoleId);

            _context.RolePermissions.RemoveRange(list);
            await _context.SaveChangesAsync();
            return list;
        }
    }
}