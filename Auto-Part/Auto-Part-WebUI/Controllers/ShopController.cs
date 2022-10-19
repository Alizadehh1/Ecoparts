using Auto_Part_WebUI.AppCode.Extensions;
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
        public IActionResult Index(int pageIndex = 1, int pageSize = 12)
        {

            var model = new ShopViewModel();
            model.Brands = db.Brands
                .Where(b => b.DeletedById == null)
                .ToList();
            model.Categories = db.Categories
                .Include(c=>c.Children.Where(c=>c.DeletedById==null))
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
        public IActionResult Brands(int id, int pageIndex = 1, int pageSize = 12)
        {
            var model = new ShopViewModel();
            model.Categories = db.Categories
                .Include(c => c.Children.Where(c => c.DeletedById == null))
                .Where(c => c.BrandId == id && c.DeletedById == null)
                .ToList();
            ViewBag.Brand = db.Brands
                .Where(c => c.Id == id && c.DeletedById == null)
                .ToList();
            model.Pricings = db.ProductPricings
                .Where(pc => pc.DeletedById == null)
                .ToList();
            model.Types = db.ProductTypes
                .Where(pt => pt.DeletedById == null)
                .ToList();
            model.Brands = db.Brands
                .Where(b => b.DeletedById == null)
                .ToList();
            model.Types = db.ProductTypes
                .Where(b => b.DeletedById == null)
                .ToList();
            var query = db.Products.Include(p => p.Category).Where(b => b.DeletedById == null && b.Category.Brand.Id == id);
            model.PagedViewModel = new PagedViewModel<Product>(query, pageIndex, pageSize);
            return View(model);
        }
        public IActionResult Categories(int id, int pageIndex = 1, int pageSize = 12)
        {
            var model = new ShopViewModel();
            model.Category = db.Categories
                .Include(c => c.Children)
                .FirstOrDefault(c => c.Id == id && c.DeletedById == null);
            model.Pricings = db.ProductPricings
                .Where(pc => pc.DeletedById == null)
                .ToList();
            model.Types = db.ProductTypes
                .Where(pt => pt.DeletedById == null)
                .ToList();
            model.Brands = db.Brands
                .Where(b => b.DeletedById == null)
                .ToList();
            model.Categories = db.Categories
                .Include(c => c.Children.Where(c => c.DeletedById == null))
                .Where(b => b.DeletedById == null)
                .ToList();
            model.Types = db.ProductTypes
                .Where(b => b.DeletedById == null)
                .ToList();
            var query = db.Products.Include(p => p.Category).Where(b => b.DeletedById == null && b.CategoryId == id);
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
            model.Products = await db.Products.Include(p => p.Category).ThenInclude(c=>c.Brand).Where(p => p.DeletedById == null && p.ForSearch.ToLower().Contains(query.ToLower())).ToListAsync();
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
            if (Request.Cookies.TryGetValue("cards", out string cards) && Request.Cookies.TryGetValue("prices", out string prices) && Request.Cookies.TryGetValue("qtys", out string qtys))
            {
                int[] productIdsFromCookie = cards.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                double[] pricesFromCookie = prices.Split(",").Where(CheckIsDouble)
                        .Select(item => double.Parse(item))
                        .ToArray();
                int[] quantitiesFromCookie = qtys.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                var allProducts = db.Products.Where(p => p.DeletedById == null).ToList();


                var products = (from p in db.Products.Where(p => p.DeletedById == null)
                                where productIdsFromCookie.Contains(p.Id) && p.DeletedById == null
                                join pr in db.ProductPricings on p.Id equals pr.ProductId
                                where pricesFromCookie.Contains(pr.Price) && p.Id == pr.ProductId && pr.DeletedById == null
                                select Tuple.Create(productIdsFromCookie, allProducts,  pricesFromCookie, quantitiesFromCookie)).ToList();

                return View(products);

            }

            return View(new List<Tuple<int[], List<Product>, double[],int[]>>());

        }

        public IActionResult Wishlist()
        {
            if (Request.Cookies.TryGetValue("cardsForWishlist", out string cardsForCookie))
            {
                int[] idsFromCookie = cardsForCookie.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                var products = from p in db.Products.Where(p => p.DeletedById == null)
                               where idsFromCookie.Contains(p.Id) && p.DeletedById == null
                               select p;

                return View(products.ToList());

            }

            return View(new List<Product>());

        }

        public async Task<IActionResult> PlaceOrder(string productIds, string totalAmount,string quantities,string prices)
        {
            int[] productId = productIds.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();
            int[] quantity = quantities.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();
            int[] price = prices.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();


            var newOrder = new Order();
            newOrder.ECoPartUserId = User.GetUserId();
            newOrder.TotalAmount = Convert.ToDouble(totalAmount);
            if (productId != null)
            {
                newOrder.OrderItems = new List<OrderItem>();
                int i = 0;
                foreach (var id in productId)
                {

                    newOrder.OrderItems.Add(new OrderItem
                    {
                        ProductId = id,
                        OrderId = newOrder.Id,
                        Quantity = quantity[i],
                        Price = price[i]
                    });
                    i++;
                }
            }
            await db.Orders.AddAsync(newOrder);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CheckIsNumber(string value)
        {
            return int.TryParse(value, out int v);
        }
        private bool CheckIsDouble(string value)
        {
            return double.TryParse(value, out double v);
        }
    }
}
