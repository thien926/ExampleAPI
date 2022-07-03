using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        
        public ICollection<User> Users { get; set; } = null!;
        public ICollection<Customer> Customers { get; set; } = null!;
        
        
    }
}