using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class UserDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage ="Email có tối đa 256 ký tự")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "Mật khẩu có tối đa 256 ký tự")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Tên là bắt buộc là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "Tên có tối đa 256 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "RoleId là bắt buộc")]
        public Guid RoleId { get; set; }

        [Required(ErrorMessage = "TenantId là bắt buộc")]
        public Guid TenantId { get; set; }
    }
}