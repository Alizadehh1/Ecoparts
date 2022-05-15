using Auto_Part_WebUI.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.Entities
{
    public class ProductPricing : HistoryEntity
    {
        public int TypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; }
    }
}
