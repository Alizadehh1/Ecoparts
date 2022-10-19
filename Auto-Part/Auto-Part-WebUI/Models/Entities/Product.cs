using Auto_Part_WebUI.AppCode.Infrastructure;
using System.Collections.Generic;

namespace Auto_Part_WebUI.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string PartCodeName { get; set; }
        public string PartCodeIds { get; set; }
        public string ShortDescription { get; set; }
        public string ImagePath { get; set; }
        public string ForSearch { get; set; }
        public int Quantity { get; set; }
        public string MainPartCodeName { get; set; }
        public virtual ICollection<ProductPricing> Pricings { get; set; }
    }
}
