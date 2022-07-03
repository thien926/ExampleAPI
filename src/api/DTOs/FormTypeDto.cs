using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class FormTypeDto
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Code là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "Code có tối đa 256 ký tự")]
        public string Code { get; set; } = null!;
        
        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Mô tả có tối đa 1000 ký tự")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "CustomerId là bắt buộc")]
        public Guid CustomerId { get; set; }

    }
}