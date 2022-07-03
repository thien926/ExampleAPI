using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public ICollection<RolePermission> RolePermissions { get; set; } = null!;
    }
}