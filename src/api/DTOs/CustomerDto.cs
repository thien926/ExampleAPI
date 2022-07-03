using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.DTOs
{
    public class CustomerDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [StringLength(maximumLength: 512, ErrorMessage = "Tên có tối đa 512 ký tự")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Họ là bắt buộc")]
        [StringLength(maximumLength: 512, ErrorMessage = "Họ có tối đa 512 ký tự")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "Email có tối đa 256 ký tự")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "Phone có tối đa 256 ký tự")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "MobilePhone là bắt buộc")]
        [StringLength(maximumLength: 256, ErrorMessage = "MobilePhone có tối đa 256 ký tự")]
        public string MobilePhone { get; set; } = null!;

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        public Genders Gender { get; set; }

        [Required(ErrorMessage = "Properties là bắt buộc")]
        public string Properties { get; set; } = null!;

        [Required(ErrorMessage = "TenantId Tenant là bắt buộc")]
        public Guid TenantId { get; set; }
    }
}