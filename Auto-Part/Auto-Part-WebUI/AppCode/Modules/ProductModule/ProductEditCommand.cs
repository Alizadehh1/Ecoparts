using Auto_Part_WebUI.AppCode.Extensions;
using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.AppCode.Modules.ProductModule
{
    public class ProductEditCommand : IRequest<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int Quantity { get; set; }
        public string ShortDescription { get; set; }
        public ProductPricing[] Pricing { get; set; }
        public string Values { get; set; }
        public string Ids { get; set; }
        public IFormFile File { get; set; }
        public string ImagePath { get; set; }
        public string ForSearch { get; set; }
        public string MainPartCodeName { get; set; }


        public class ProductEditCommandHandler : IRequestHandler<ProductEditCommand, Product>
        {
            readonly ECoPartDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;

            public ProductEditCommandHandler(ECoPartDbContext db,
                IActionContextAccessor ctx,
                IWebHostEnvironment env)
            {
                this.db = db;
                this.ctx = ctx;
                this.env = env;
            }

            public async Task<Product> Handle(ProductEditCommand request, CancellationToken cancellationToken)
            {
                if (ctx.ModelIsValid())
                {
                    if (request.File == null && string.IsNullOrEmpty(request.ImagePath))
                    {
                        ctx.AddModelError("ImagePath", "Image Cannot be empty");
                    }
                    var product = await db.Products
                    .Include(p => p.Pricings)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
                    if (product == null)
                    {
                        return null;
                    }
                    string oldFileName = product.ImagePath;
                    if (request.File != null)
                    {
                        string fileExtension = Path.GetExtension(request.File.FileName);
                        string name = $"product-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", name);
                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            request.File.CopyTo(fs);
                        }
                        product.ImagePath = name;
                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", oldFileName);
                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }
                    product.Name = request.Name;
                    product.CategoryId = request.CategoryId;
                    product.BrandId = request.BrandId;
                    product.ShortDescription = request.ShortDescription;
                    product.Quantity = request.Quantity;
                    product.MainPartCodeName = request.MainPartCodeName;
                    if (request.Values != null && request.Values.Length > 0)
                    {
                        product.PartCodeName = request.Values;
                        product.ForSearch = request.MainPartCodeName + request.Name + request.Values;
                    }

                    if (request.Ids != null && request.Ids.Length > 0)
                    {
                        product.PartCodeIds = request.Ids;
                    }

                    if (request.Pricing != null && request.Pricing.Length > 0)
                    {
                        product.Pricings = new List<Models.Entities.ProductPricing>();

                        foreach (var pricing in request.Pricing)
                        {
                            product.Pricings.Add(new Models.Entities.ProductPricing
                            {
                                TypeId = pricing.TypeId,
                                Price = pricing.Price
                            });
                        }
                    }
                    try
                    {
                        await db.SaveChangesAsync(cancellationToken);
                        return product;
                    }
                    catch (Exception ex)
                    {

                        ctx.AddModelError("Name", "Xeta bash verdi,Biraz sonra yeniden yoxlayin");

                        return product;
                    }
                l1:
                    return null;
                }
                return null;
            }
        }
    }
}
