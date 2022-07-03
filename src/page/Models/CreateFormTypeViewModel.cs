using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using page.Entities;

namespace page.Models
{
    public class CreateFormTypeViewModel
    {
        public FormType formType { get; set; }
        
        public IEnumerable<Customer>? customers { get; set; }
        
    }
}