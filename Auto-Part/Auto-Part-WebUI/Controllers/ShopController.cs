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
    [Authorize(Policy = "shop")]
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
        [Authorize(Policy = "shop.details")]
        public IActionResult Details(int id)
        {
            var product = db.Products
                .Include(p => p.Pricings)
                .ThenInclude(p=>p.ProductType)
                .Include(p=>p.Category)
                .ThenInclude(p=>p.Brand)
                .FirstOrDefault(p => p.DeletedById == null && p.Id==id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Types = db.ProductTypes
                .Where(t => t.DeletedById == null)
                .ToList();
            return View(product);
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
        [Authorize(Policy = "shop.search")]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index", "Shop");
            }
            List<Product> products = await db.Products.Where(p => p.ForSearch.ToLower().Contains(query.ToLower())).ToListAsync();
            return View(products);
        }
        [Authorize(Policy = "shop.searchPartial")]
        public async Task<IActionResult> SearchPartial(string query)
        {
            List<Product> products = await db.Products.Where(p => p.ForSearch.ToLower().Contains(query.ToLower())).ToListAsync();
            return PartialView("_SearchPartialView", products);
        }

    }
}
