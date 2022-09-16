using Auto_Part_WebUI.Models.Entities;
using Auto_Part_WebUI.Models.Entities.Membership;
using Auto_Part_WebUI.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.ViewModels
{
    public class ProfileViewModel
    {
        public ECoPartUser ECoPartUser { get; set; }
        public List<Order> Order { get; set; }
        public ChangePasswordFormModel ChangePasswordFormModel { get; set; }
    }
}
