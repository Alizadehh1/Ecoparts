using Auto_Part_WebUI.Models.DataContexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Controllers
{
    public class HomeController : Controller
    {
        readonly ECoPartDbContext db;

        public HomeController(ECoPartDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
