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

namespace Auto_Part_WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductPartCodesController : Controller
    {
        private readonly ECoPartDbContext db;
        private readonly UserManager<ECoPartUser> userManager;

        public ProductPartCodesController(ECoPartDbContext db, UserManager<ECoPartUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.productpartcodes.index")]
        public async Task<IActionResult> Index()
        {
            
            return View(await db.ProductPartCodes
                .Where(ppc=>ppc.DeletedById==null)
                .ToListAsync());
        }
        [Authorize(Policy = "admin.productpartcodes.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productPartCode = await db.ProductPartCodes
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById==null);
            if (productPartCode == null)
            {
                return NotFound();
            }

            return View(productPartCode);
        }
        [Authorize(Policy = "admin.productpartcodes.create")]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(db.Products, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.productpartcodes.create")]
        public async Task<IActionResult> Create([Bind("ProductId,Code,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] ProductPartCode productPartCode)
        {
            if (ModelState.IsValid)
            {
                db.Add(productPartCode);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(db.Products, "Id", "Id", productPartCode.ProductId);
            return View(productPartCode);
        }
        [Authorize(Policy = "admin.productpartcodes.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productPartCode = await db.ProductPartCodes
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (productPartCode == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(db.Products, "Id", "Id", productPartCode.ProductId);
            return View(productPartCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.productpartcodes.edit")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Code,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] ProductPartCode productPartCode)
        {
            if (id != productPartCode.Id || productPartCode.DeletedById == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(productPartCode);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductPartCodeExists(productPartCode.Id))
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
            ViewData["ProductId"] = new SelectList(db.Products, "Id", "Id", productPartCode.ProductId);
            return View(productPartCode);
        }

        [HttpPost]
        [Authorize(Policy = "admin.productpartcodes.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = db.ProductPartCodes.FirstOrDefault(b => b.Id == id && b.DeletedById == null);
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

        private bool ProductPartCodeExists(int id)
        {
            return db.ProductPartCodes.Any(e => e.Id == id);
        }
    }
}
