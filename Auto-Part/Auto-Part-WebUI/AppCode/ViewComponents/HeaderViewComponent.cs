using Auto_Part_WebUI.Models.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Vapie.WebUI.AppCode.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ECoPartDbContext db;

        public HeaderViewComponent(ECoPartDbContext db)
        {
            this.db = db;
        }
        public IViewComponentResult Invoke()
        {
            var data = db.Categories
                .Where(c=>c.DeletedById==null)
               .Select(c => new
               {
                   Id = c.Id,
                   Name = c.ParentId == null ? c.Name : $"- {c.Name}"
               })
               .ToList();
            ViewBag.Categories = new SelectList(data, "Id", "Name");

            return View();
        }
    }
}
