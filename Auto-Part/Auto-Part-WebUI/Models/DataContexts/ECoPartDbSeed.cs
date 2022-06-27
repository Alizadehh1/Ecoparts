using Auto_Part_WebUI.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Auto_Part_WebUI.Models.DataContexts
{
    public static class ECoPartDbSeed
    {
        static internal void InitDb(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ECoPartDbContext>();
                db.Database.Migrate();
                InitBrands(db);
            }
        }

        private static void InitBrands(ECoPartDbContext db)
        {
            if (!db.Brands.Any())
            {
                db.Brands.AddRange(new[]
                {
                    new Brand{Name="Toyota/Lexus"},
                    new Brand{Name="Nissan/Infiniti"},
                    new Brand{Name="Mitsubishi"},
                    new Brand{Name="Hyundai/Kia"},
                    new Brand{Name="Mercedes"},
                    new Brand{Name="Bmw"},
                    new Brand{Name="Volkswagen"},
                    new Brand{Name="Audi"},
                    new Brand{Name="Land Rover"},
                    new Brand{Name="Ford"}
                });

                db.SaveChanges();
            }
        }
    }
}
