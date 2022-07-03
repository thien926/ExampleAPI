using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace page.Entities
{
    public enum Genders 
    {
        Male,
        Female,
        Others
    }
    public class Customer
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

        public List<SelectListItem> CheeseGenders { get; set; }
        
        public Customer() 
        {
            CheeseGenders = new List<SelectListItem>();

            Array enumValueArray = Enum.GetValues(typeof(Genders)); 
            foreach (int enumValue in enumValueArray)  
            {  
                CheeseGenders.Add(new SelectListItem {
                    Value = enumValue.ToString(),
                    Text = Enum.GetName(typeof(Genders), enumValue).ToString()
                });
            }  

            // CheeseGenders.Add(new SelectListItem {
            //     Value = ((int) Genders.Male).ToString(),
            //     Text = Genders.Male.ToString()
            // });

            // CheeseGenders.Add(new SelectListItem {
            //     Value = ((int) Genders.Female).ToString(),
            //     Text = Genders.Female.ToString()
            // });

            // CheeseGenders.Add(new SelectListItem {
            //     Value = ((int) Genders.Others).ToString(),
            //     Text = Genders.Others.ToString()
            // });
        }
    }
}