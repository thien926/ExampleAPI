using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        
        public Guid PermissionId { get; set; }
        
        public virtual Role Role { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
}