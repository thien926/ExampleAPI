using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace page.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "Tên có tối đa 256 ký tự")]
        public string Name { get; set; } = null!;

        public string? Detail { get; set; }
        
        
    }
}