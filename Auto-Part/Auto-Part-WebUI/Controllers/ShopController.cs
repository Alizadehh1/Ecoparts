using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities;
using Auto_Part_WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Controllers
{
    public class ShopController : Controller
    {
        readonly ECoPartDbContext db;

        public ShopController(ECoPartDbContext db)
        {
            this.db = db;
        }
        [Authorize(Policy = "shop.index")]
        public IActionResult Index()
        {
            var model = db.Brands
                .Where(b => b.DeletedById == null)
                .ToList();
            return View(model);
        }
        [Authorize(Policy = "shop.categories")]
        public IActionResult Categories(int id)
        {
            var model = new ShopViewModel();
            model.Categories=db.Categories
                .Include(c => c.Children)
                .Where(c => c.BrandId == id && c.DeletedById == null)
                .ToList();
            ViewBag.Brand = db.Brands
                .Where(c=>c.Id==id && c.DeletedById==null)
                .ToList();
            return View(model);
        }

    }
}
