using Auto_Part_WebUI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.DataContexts
{
    public class ECoPartDbContext : DbContext
    {
        public ECoPartDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Brand> Brands { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductPricing>(e =>
            {
                e.HasKey(k => new { k.ProductId, k.TypeId });
            });
        }
    }
}
