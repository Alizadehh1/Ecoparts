﻿using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities;
using Auto_Part_WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public IActionResult Index(int pageIndex = 1, int pageSize = 5)
        {

            var model = new ShopViewModel();
            model.Brands = db.Brands
                .Where(b => b.DeletedById == null)
                .ToList();
            model.Pricings = db.ProductPricings
                .Where(pc => pc.DeletedById == null)
                .ToList();
            model.Types = db.ProductTypes
                .Where(pt => pt.DeletedById == null)
                .ToList();
            var query = db.Products.Include(p => p.Category).Where(b => b.DeletedById == null);
            model.PagedViewModel = new PagedViewModel<Product>(query, pageIndex, pageSize);
            return View(model);
        }
        [Authorize(Policy = "shop.details")]
        public IActionResult Details(int id)
        {
            var model = new ShopViewModel();
            model.Product = db.Products
                .Include(p => p.Pricings)
                .Include(p => p.Category)
                .ThenInclude(p => p.Brand)
                .FirstOrDefault(p => p.DeletedById == null && p.Id == id);
            model.Types = db.ProductTypes
                .Where(pt => pt.DeletedById == null)
                .ToList();
            model.Pricings = db.ProductPricings
                .Where(pc => pc.DeletedById == null)
                .ToList();
            model.Products = db.Products
                .Include(p => p.Category)
                .Where(b => b.DeletedById == null && b.Category.Brand.Id == model.Product.Category.Brand.Id && b.Id != id)
                .ToList();
            
            return View(model);
        }
        [Authorize(Policy = "shop.categories")]
        public IActionResult Categories(int id, int pageIndex = 1, int pageSize = 5)
        {
            var model = new ShopViewModel();
            model.Categories=db.Categories
                .Include(c => c.Children)
                .Where(c => c.BrandId == id && c.DeletedById == null)
                .ToList();
            ViewBag.Brand = db.Brands
                .Where(c=>c.Id==id && c.DeletedById==null)
                .ToList();
            var query = db.Products.Include(p => p.Category).Where(b => b.DeletedById == null && b.Category.Brand.Id == id);
            model.PagedViewModel = new PagedViewModel<Product>(query, pageIndex, pageSize);
            return View(model);
        }
        [Authorize(Policy = "shop.search")]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index", "Shop");
            }
            var model = new ShopViewModel();
            model.Products = await db.Products.Include(p => p.Category).Where(p => p.DeletedById==null && p.ForSearch.ToLower().Contains(query.ToLower())).ToListAsync();
            model.Pricings = db.ProductPricings
                .Where(pc => pc.DeletedById == null)
                .ToList();
            model.Types = db.ProductTypes
                .Where(pt => pt.DeletedById == null)
                .ToList();
            return View(model);
        }
        [Authorize(Policy = "shop.searchPartial")]
        public async Task<IActionResult> SearchPartial(string query)
        {
            var model = new ShopViewModel();
            model.Products = await db.Products.Where(p => p.DeletedById == null && p.ForSearch.ToLower().Contains(query.ToLower())).ToListAsync();
            return PartialView("_SearchPartialView", model);
        }


        public IActionResult Basket()
        {
            if (Request.Cookies.TryGetValue("cards", out string cards))
            {
                var result = JsonConvert.DeserializeObject(cards);

                //var products = from p in db.Products.Where(p => p.DeletedById == null)
                //               where idsFromCookie.Contains(p.Id) && p.DeletedById == null
                //               select p;
                return View(result);

            }

            return View(new List<Product>());

        }

        private bool CheckIsNumber(string value)
        {
            return int.TryParse(value, out int v);
        }
    }
}
