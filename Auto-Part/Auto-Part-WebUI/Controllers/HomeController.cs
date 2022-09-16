using Auto_Part_WebUI.AppCode.Modules.SubscribeModule;
using Auto_Part_WebUI.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Modules.SubscribeModule;

namespace Auto_Part_WebUI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        readonly ECoPartDbContext db;
        private readonly IMediator mediator;

        public HomeController(ECoPartDbContext db,IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Subscribe(SubscribeCreateCommand command)
        {

            var response = await mediator.Send(command);

            return Json(response);
        }

        [HttpGet]
        [Route("/subscribe-confirm")]
        public async Task<IActionResult> SubscribeConfirm(SubscribeConfirmCommand command)
        {
            var response = await mediator.Send(command);

            return View(response);
        }
    }
}
