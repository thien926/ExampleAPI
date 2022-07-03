using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public enum Genders 
    {
        Male,
        Female,
        Others
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string MobilePhone { get; set; } = null!;
        public Genders Gender { get; set; }
        public string Properties { get; set; } = null!;
        public Guid TenantId { get; set; }
        
        public Tenant Tenant { get; set; } = null!;
        public ICollection<FormType> FormTypes { get; set; } = null!;
    }
}