﻿using Auto_Part_WebUI.AppCode.Infrastructure;
using Auto_Part_WebUI.AppCode.Modules.ProductModule;
using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities;
using Auto_Part_WebUI.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ECoPartDbContext db;
        private readonly IMediator mediator;

        public ProductsController(ECoPartDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
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
            ViewBag.Codes = db.PartCodes.Where(ppc => ppc.DeletedById == null)
               .ToList();
            return View(product);
        }

        [HttpPost]
        [Authorize(Policy = "admin.products.edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, ProductEditCommand model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            var product = await mediator.Send(model);


            return RedirectToAction(nameof(Index));
        }

    }
}