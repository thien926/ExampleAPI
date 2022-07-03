using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using page.Entities;

namespace page.Models
{
    public class CreateRoleViewModel
    {
        public Role role { get; set; }
        
        public IEnumerable<Permission>? permissions { get; set; }
        
        
    }
}