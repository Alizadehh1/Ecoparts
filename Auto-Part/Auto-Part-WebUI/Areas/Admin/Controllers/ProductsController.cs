using Auto_Part_WebUI.AppCode.Infrastructure;
using Auto_Part_WebUI.AppCode.Modules.ProductModule;
using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities;
using Auto_Part_WebUI.Models.Entities.Membership;
using Auto_Part_WebUI.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ECoPartDbContext db;
        private readonly IMediator mediator;
        private readonly UserManager<ECoPartUser> userManager;

        public ProductsController(ECoPartDbContext db, IMediator mediator, UserManager<ECoPartUser> userManager)
        {
            this.db = db;
            this.mediator = mediator;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.products.index")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5)
        {
            var query = db.Products
                .Include(p => p.Category)
                .Where(p => p.DeletedById == null);
            var pagedModel = new PagedViewModel<Product>(query, pageIndex, pageSize);
            return View(pagedModel);
        }
        [Authorize(Policy = "admin.products.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.Category)
                .ThenInclude(c => c.Brand)
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);

            ViewBag.Pricings = db.ProductPricings
                .Where(pc => pc.DeletedById == null)
                .ToList();
            ViewBag.Types = db.ProductTypes
                .Where(pt => pt.DeletedById == null)
                .ToList();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Policy = "admin.products.create")]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categories.Where(c => c.DeletedById == null), "Id", "Name");
            ViewData["Types"] = new SelectList(db.ProductTypes.Where(c => c.DeletedById == null), "Id", "Name");
            ViewBag.Codes = db.PartCodes.Where(ppc => ppc.DeletedById == null)
                .OrderBy(pc => pc.Name)
               .ToList();
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "admin.products.create")]
        public async Task<IActionResult> Create(ProductCreateCommand command)
        {
            var product = await mediator.Send(command);


            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }
        public async Task<IActionResult> SearchInput(string key)
        {
            List<PartCode> partCodes = new List<PartCode>();
            if (key != null)
            {
                partCodes = await db.PartCodes
                .Where(p => p.Name.Contains(key) && p.DeletedById==null)
                .ToListAsync();
            }
            return PartialView("_PartCodeListPartial", partCodes);
        }

        [Authorize(Policy = "admin.products.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.Pricings)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.Categories.Where(s => s.DeletedById == null), "Id", "Name", product.CategoryId);
            ViewData["Types"] = new SelectList(db.ProductTypes.Where(s => s.DeletedById == null), "Id", "Name");
            ViewBag.SelectedCodes = product.PartCodeIds.Split(",");
            ViewBag.Codes = db.PartCodes.Where(ppc => ppc.DeletedById == null)
                .OrderBy(pc=>pc.Name)
               .ToList();
            return View(product);
        }

        [HttpPost]
        [Authorize(Policy = "admin.products.edit")]
        public async Task<IActionResult> Edit(ProductEditCommand model)
        {
            var product = await mediator.Send(model);
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [Authorize(Policy = "admin.brands.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = db.Products.FirstOrDefault(b => b.Id == id && b.DeletedById == null);
            if (entity == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Movcud deyil"
                });
            }
            var user = await userManager.GetUserAsync(User);
            entity.DeletedById = user.Id;
            entity.DeletedDate = DateTime.UtcNow.AddHours(4);
            db.SaveChanges();
            return Json(new
            {
                error = false,
                message = "Ugurla silindi"
            });
        }

    }
}