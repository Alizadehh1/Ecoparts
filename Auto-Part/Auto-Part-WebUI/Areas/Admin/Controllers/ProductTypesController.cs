using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Auto_Part_WebUI.Models.Entities.Membership;
using Auto_Part_WebUI.Models.ViewModels;

namespace Auto_Part_WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ECoPartDbContext db;
        private readonly UserManager<ECoPartUser> userManager;

        public ProductTypesController(ECoPartDbContext db, UserManager<ECoPartUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.producttypes.index")]
        public async Task<IActionResult> Index(int pageIndex=1,int pageSize=10)
        {
            var query = db.ProductTypes
                .Where(pt => pt.DeletedById == null);
            var pagedModel = new PagedViewModel<ProductType>(query, pageIndex, pageSize);
            return View(pagedModel);
        }
        [Authorize(Policy = "admin.producttypes.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await db.ProductTypes
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }
        [Authorize(Policy = "admin.producttypes.create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.producttypes.create")]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                db.Add(productType);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }
        [Authorize(Policy = "admin.producttypes.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await db.ProductTypes.FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.producttypes.edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] ProductType productType)
        {
            if (id != productType.Id || productType.DeletedById != null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(productType);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        [HttpPost]
        [Authorize(Policy = "admin.producttypes.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = db.ProductTypes.FirstOrDefault(b => b.Id == id && b.DeletedById == null);
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

        private bool ProductTypeExists(int id)
        {
            return db.ProductTypes.Any(e => e.Id == id);
        }
    }
}
