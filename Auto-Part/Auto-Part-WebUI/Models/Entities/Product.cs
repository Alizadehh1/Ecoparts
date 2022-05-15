using Auto_Part_WebUI.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string ShortDescription { get; set; }
        public string ImagePath { get; set; }
        public ICollection<ProductPartCode> ProductPartCodes { get; set; }
        public ICollection<ProductPricing> ProductPricings { get; set; }
    }
}
