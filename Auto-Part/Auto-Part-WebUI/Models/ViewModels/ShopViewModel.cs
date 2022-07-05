using Auto_Part_WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.ViewModels
{
    public class ShopViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public List<ProductType> Types { get; set; }
        public List<ProductPricing> Pricings { get; set; }
        public PagedViewModel<Product> PagedViewModel { get; set; }
    }
}
