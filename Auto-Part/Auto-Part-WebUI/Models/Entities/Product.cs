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
        //public int PartCodeId { get; set; }
        //public virtual PartCode PartCode { get; set; }
        public string PartCodeName { get; set; }
        public string PartCodeIds { get; set; }
        public string ShortDescription { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<ProductPricing> Pricings { get; set; }
    }
}
