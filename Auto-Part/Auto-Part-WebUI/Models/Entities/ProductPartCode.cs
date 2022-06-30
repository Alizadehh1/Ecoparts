using Auto_Part_WebUI.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.Entities
{
    public class ProductPartCode : BaseEntity
    {
        public int? ProductId { get; set; }
        public string Code { get; set; }
    }
}
