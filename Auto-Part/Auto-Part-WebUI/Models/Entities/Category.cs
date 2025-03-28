﻿using Auto_Part_WebUI.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<Product> Product { get; set; }

    }
}
