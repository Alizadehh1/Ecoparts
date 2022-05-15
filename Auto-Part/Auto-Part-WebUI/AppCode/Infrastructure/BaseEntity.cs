using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.AppCode.Infrastructure
{
    public class BaseEntity : HistoryEntity
    {
        public int Id { get; set; }
    }
    public class HistoryEntity
    {
        public int? CreatedById { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public int? DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
