using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.FormModels
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "'Email' Xanasını boş saxlamayın!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "'Şifrə' Xanasını boş saxlamayın!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "'Şifrənın təkrarı' Xanasını boş saxlamayın!")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Şifrələr Uyğun Deyil!")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "'Mağaza adı' Xanasını boş saxlamayın!")]
        public string StoreName { get; set; }
        [Required(ErrorMessage = "'Ad' Xanasını boş saxlamayın!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "'Soyad' Xanasını boş saxlamayın!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "'Telefon nömrəsi' Xanasını boş saxlamayın!")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "'İstifadəçi adı' Xanasını boş saxlamayın!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "'Adres' Xanasını boş saxlamayın!")]
        public string Adress { get; set; }

    }
}
