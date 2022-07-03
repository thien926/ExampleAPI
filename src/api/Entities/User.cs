using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        // public string Password { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public Guid RoleId { get; set; }
        public Guid TenantId { get; set; }
        
        public virtual Role Role { get; set; } = null!;
        public virtual Tenant Tenant { get; set; } = null!;
    }
}