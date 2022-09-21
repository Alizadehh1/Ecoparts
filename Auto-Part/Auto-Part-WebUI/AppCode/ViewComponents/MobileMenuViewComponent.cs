using Auto_Part_WebUI.Models.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.AppCode.ViewComponents
{
    public class MobileMenuViewComponent : ViewComponent
    {
        private readonly ECoPartDbContext db;

        public MobileMenuViewComponent(ECoPartDbContext db)
        {
            this.db = db;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.PopularCars = db.PopularCars
                .Include(pc => pc.Brand)
                .Where(c => c.DeletedById == null)
                .ToList();


            return View();
        }
    }
}
