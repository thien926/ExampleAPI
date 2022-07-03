using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence
{
    public class PermissionRepository
    {
        private readonly RentContext _permissionContext;
        public PermissionRepository(RentContext permissionContext)
        {
            _permissionContext = permissionContext;
        }

        public async Task<IEnumerable<Permission>> GetPermissions()
        {
            return await _permissionContext.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetPermission(Guid Id) 
        {
            return await _permissionContext.Permissions.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<Permission> CreatePermission(Permission Permission)
        {
            _permissionContext.Permissions.Add(Permission);
            await _permissionContext.SaveChangesAsync();
            return Permission;
        }

        public async Task<Permission> UpdatePermission(Permission Permission)
        {
            _permissionContext.Permissions.Update(Permission);
            await _permissionContext.SaveChangesAsync();
            return Permission;
        }

        public async Task<Permission> DeletePermission(Permission Permission)
        {
            _permissionContext.Permissions.Remove(Permission);
            await _permissionContext.SaveChangesAsync();
            return Permission;
        }

        public async Task<IEnumerable<Permission>> GetPermissionsWithPermissionIds(IEnumerable<Guid> list)
        {
            var query = _permissionContext.Permissions.AsQueryable();
            query = query.Where(m => list.Contains(m.Id));
            return await query.ToListAsync();
        }

        public async Task<Permission?> GetPermissionFromName(string name)
        {
            return await _permissionContext.Permissions.FirstOrDefaultAsync(m => m.Name == name);
        }
    }
}