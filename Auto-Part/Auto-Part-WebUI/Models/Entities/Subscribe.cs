using Auto_Part_WebUI.AppCode.Infrastructure;
using System;

namespace Auto_Part_WebUI.Models.Entities
{
    public class Subscribe : BaseEntity
    {
        public string Email { get; set; }
        public bool EmailSended { get; set; } = false;
        public DateTime? AppliedDate { get; set; }
    }
}
