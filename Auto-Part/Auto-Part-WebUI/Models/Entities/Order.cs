using Auto_Part_WebUI.AppCode.Infrastructure;
using Auto_Part_WebUI.Models.Entities.Membership;
using System.Collections.Generic;

namespace Auto_Part_WebUI.Models.Entities
{
    public class Order : BaseEntity
    {
        public bool isConfirmed { get; set; }
        public string ECoPartUserId { get; set; }
        public virtual ECoPartUser ECoPartUser { get; set; }
        public double TotalAmount { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
