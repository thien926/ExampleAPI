using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using page.Entities;

namespace page.Models
{
    public class CreateCustomerViewModel
    {
        public Customer customer { get; set; }
        
        public IEnumerable<Tenant>? tenants { get; set; }
        
    }
}