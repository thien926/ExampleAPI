using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class FormType
    {
        public Guid Id { get; set; }
        
        public string Code { get; set; } = null!;
        
        public string Description { get; set; } = null!;

        public Guid CustomerId { get; set; }
        
        public virtual Customer Customer { get; set; } = null!;
    }
}