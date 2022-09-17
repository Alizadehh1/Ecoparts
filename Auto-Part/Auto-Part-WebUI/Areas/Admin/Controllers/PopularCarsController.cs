﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Auto_Part_WebUI.Models.Entities.Membership;
using Microsoft.AspNetCore.Authorization;

namespace Auto_Part_WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PopularCarsController : Controller
    {
        private readonly ECoPartDbContext db;
        private readonly UserManager<ECoPartUser> userManager;

        public PopularCarsController(ECoPartDbContext db, UserManager<ECoPartUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.popularCars.index")]
        public async Task<IActionResult> Index()
        {
            var eCoPartDbContext = db.PopularCars.Where(pc=>pc.DeletedById==null).Include(p => p.Brand);
            return View(await eCoPartDbContext.ToListAsync());
        }
        [Authorize(Policy = "admin.popularCars.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var popularCar = await db.PopularCars
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById==null);
            if (popularCar == null)
            {
                return NotFound();
            }

            return View(popularCar);
        }
        [Authorize(Policy = "admin.popularCars.create")]
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(db.Brands.Where(b=>b.DeletedById==null), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.popularCars.create")]
        public async Task<IActionResult> Create(PopularCar popularCar)
        {
            if (ModelState.IsValid)
            {
                db.Add(popularCar);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(db.Brands.Where(b => b.DeletedById == null), "Id", "Name", popularCar.BrandId);
            return View(popularCar);
        }
        [Authorize(Policy = "admin.popularCars.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var popularCar = await db.PopularCars.FindAsync(id);
            
            if (popularCar == null || popularCar.DeletedById!=null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(db.Brands.Where(b => b.DeletedById == null), "Id", "Name", popularCar.BrandId);
            return View(popularCar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.popularCars.edit")]
        public async Task<IActionResult> Edit(int id,PopularCar popularCar)
        {
            if (id != popularCar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(popularCar);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PopularCarExists(popularCar.Id))
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
            ViewData["BrandId"] = new SelectList(db.Brands.Where(b => b.DeletedById == null), "Id", "Name", popularCar.BrandId);
            return View(popularCar);
        }

        [HttpPost]
        [Authorize(Policy = "admin.popularCars.delete")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = db.PopularCars.FirstOrDefault(b => b.Id == id && b.DeletedById == null);
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

        private bool PopularCarExists(int id)
        {
            return db.PopularCars.Any(e => e.Id == id);
        }
    }
}
