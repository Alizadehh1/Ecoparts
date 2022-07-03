using Auto_Part_WebUI.AppCode.Infrastructure;
using Auto_Part_WebUI.AppCode.Modules.ProductModule;
using Auto_Part_WebUI.Models.DataContexts;
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
        public async Task<IActionResult> Index()
        {
            var autopartDbContext = await db.Products
                .Include(p => p.Category)
                .Where(p => p.DeletedById == null)
                .ToListAsync();
            return View(autopartDbContext);
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
                .ThenInclude(c=>c.Brand)
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedById==null);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Policy = "admin.products.create")]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categories.Where(c=>c.DeletedById==null), "Id", "Name");
            ViewData["Types"] = new SelectList(db.ProductTypes.Where(c => c.DeletedById == null), "Id", "Name");
            ViewBag.Codes = db.PartCodes.Where(ppc => ppc.DeletedById == null)
               .ToList();
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "admin.products.create")]
        public async Task<IActionResult> Create(ProductCreateCommand command)
        {
            var product = await mediator.Send(command);


            if (product?.ValidationResult != null && !product.ValidationResult.IsValid)
            {
                return Json(product.ValidationResult);
            }

            return Json(new CommandJsonResponse(false, $"Ugurlu emeliyyat, yeni mehsulun kodu:{product.Product.Id}"));
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

        //[HttpPost]
        //[Authorize(Policy = "admin.products.edit")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(ProductEditCommand model)
        //{

        //    var product = await mediator.Send(model);

        //    if (product?.ValidationResult != null && !product.ValidationResult.IsValid)
        //    {
        //        return Json(product.ValidationResult);
        //    }

        //    return Json(new CommandJsonResponse(false, $"Ugurlu emeliyyat, yeni mehsulun kodu:{product.Product.Id}"));
        //}

    }
}
