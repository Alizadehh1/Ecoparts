using Auto_Part_WebUI.Models.Entities;
using Auto_Part_WebUI.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.ViewModels
{
    public class OrderViewModel
    {
        public List<ECoPartUser> Users { get; set; }
        public List<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
        public Order Order { get; set; }
        public PagedViewModel<Order> PagedViewModel { get; set; }
    }
}
