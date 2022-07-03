using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace page.Entities
{
    public class Permission
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "Tên có tối đa 256 ký tự")]
        public string Name { get; set; } = null!;
    }
}