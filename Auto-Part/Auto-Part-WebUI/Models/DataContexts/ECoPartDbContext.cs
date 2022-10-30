using Auto_Part_WebUI.Models.Entities;
using Auto_Part_WebUI.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Models.DataContexts
{
    public class ECoPartDbContext : IdentityDbContext<ECoPartUser, ECoPartRole, int, ECoPartUserClaim, ECoPartUserRole, ECoPartUserLogin, ECoPartRoleClaim, ECoPartUserToken>
    {
        public ECoPartDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PartCode> PartCodes { get; set; }
        public DbSet<ProductPricing> ProductPricings { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<PopularCar> PopularCars { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductPricing>(e =>
            {
                e.HasKey(k => new { k.ProductId, k.TypeId });
            });

            builder.Entity<ECoPartUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });
            builder.Entity<ECoPartRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });
            builder.Entity<ECoPartUserRole>(e =>
            {
                e.ToTable("UserRoles", "Membership");
            });
            builder.Entity<ECoPartUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });
            builder.Entity<ECoPartRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");
            });
            builder.Entity<ECoPartUserLogin>(e =>
            {
                e.ToTable("UserLogins", "Membership");
            });
            builder.Entity<ECoPartUserToken>(e =>
            {
                e.ToTable("UserTokens", "Membership");
            });
        }


    }
}
