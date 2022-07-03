using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage ="Email có tối đa 256 ký tự")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "Mật khẩu có tối đa 256 ký tự")]
        public string Password { get; set; } = null!;
    }
}