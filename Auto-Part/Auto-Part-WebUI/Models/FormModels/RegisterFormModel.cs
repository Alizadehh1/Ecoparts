using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.FormModels
{
    public class RegisterFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Şifrələr Uyğun Deyil!")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string StoreName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

    }
}
