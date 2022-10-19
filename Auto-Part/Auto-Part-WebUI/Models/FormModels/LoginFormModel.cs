using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.FormModels
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = "'Email (İstifadəçi adı)' Xanasını boş saxlamayın!")]
        
        public string UserName { get; set; }
        [Required(ErrorMessage = "'Şifrə' Xanasını boş saxlamayın!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
