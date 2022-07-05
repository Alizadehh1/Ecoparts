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
    public class PartCodesController : Controller
    {
        private readonly ECoPartDbContext db;
        private readonly UserManager<ECoPartUser> userManager;

        public PartCodesController(ECoPartDbContext db, UserManager<ECoPartUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.partcodes.index")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5)
        {
            var query = db.PartCodes
                .Where(ppc => ppc.DeletedById == null);
            var pagedModel = new PagedViewModel<PartCode>(query, pageIndex, pageSize);
            return View(pagedModel);
        }
        [Authorize(Policy = "admin.partcodes.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productPartCode = await db.PartCodes
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (productPartCode == null)
            {
                return NotFound();
            }

            return View(productPartCode);
        }
        [Authorize(Policy = "admin.partcodes.create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.partcodes.create")]
        public async Task<IActionResult> Create(PartCode partCode)
        {
            if (ModelState.IsValid)
            {
                db.Add(partCode);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partCode);
        }
        [Authorize(Policy = "admin.partcodes.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partCode = await db.PartCodes
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById == null);
            if (partCode == null)
            {
                return NotFound();
            }
            return View(partCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.partcodes.edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] PartCode partCode)
        {
            if (id != partCode.Id || partCode.DeletedById != null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(partCode);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductPartCodeExists(partCode.Id))
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

            return View(partCode);
        }

        [HttpPost]
        [Authorize(Policy = "admin.partcodes.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = db.PartCodes.FirstOrDefault(b => b.Id == id && b.DeletedById == null);
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
            return db.PartCodes.Any(e => e.Id == id);
        }
    }
}
