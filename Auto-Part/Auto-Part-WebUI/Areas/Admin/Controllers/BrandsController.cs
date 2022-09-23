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
{   [AllowAnonymous]
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly ECoPartDbContext db;
        private readonly UserManager<ECoPartUser> userManager;

        public BrandsController(ECoPartDbContext db, UserManager<ECoPartUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.brands.index")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            var query = db.Brands
                .Where(b => b.DeletedById == null);
            var pagedModel = new PagedViewModel<Brand>(query, pageIndex, pageSize);
            return View(pagedModel);
        }
        [Authorize(Policy = "admin.brands.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await db.Brands
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById==null);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }
        [Authorize(Policy = "admin.brands.create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.brands.create")]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Add(brand);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }
        [Authorize(Policy = "admin.brands.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await db.Brands
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.brands.edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] Brand brand)
        {
            if (id != brand.Id || brand.DeletedById!=null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(brand);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            return View(brand);
        }
        [HttpPost]
        [Authorize(Policy = "admin.brands.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = db.Brands.FirstOrDefault(b => b.Id == id && b.DeletedById == null);
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

        private bool BrandExists(int id)
        {
            return db.Brands.Any(e => e.Id == id);
        }
    }
}
