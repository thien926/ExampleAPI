using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using page.Entities;

namespace page.Models
{
    public class UpdateUserViewModel
    {
        public UserUpdate user { get; set; }
        
        public IEnumerable<Tenant>? tenants { get; set; }
        
        public IEnumerable<Role>? roles { get; set; }
    }
}