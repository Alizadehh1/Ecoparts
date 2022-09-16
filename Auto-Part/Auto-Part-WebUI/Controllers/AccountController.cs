using Auto_Part_WebUI.AppCode.Extensions;
using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities.Membership;
using Auto_Part_WebUI.Models.FormModels;
using Auto_Part_WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Part_WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ECoPartDbContext db;
        private readonly SignInManager<ECoPartUser> signInManager;
        private readonly UserManager<ECoPartUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IActionContextAccessor ctx;

        public AccountController(ECoPartDbContext db,SignInManager<ECoPartUser> signInManager, UserManager<ECoPartUser> userManager,
            IConfiguration configuration,
            IActionContextAccessor ctx)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.ctx = ctx;
        }
        [Route("/signin.html")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }
        [Route("/profile.html")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.GetUserId();
            var viewModel = new ProfileViewModel();
            viewModel.Order = db.Orders.Where(o => o.ECoPartUserId == userId).Include(o => o.OrderItems).ToList();
            viewModel.ECoPartUser = await userManager.FindByIdAsync(userId);
            return View(viewModel);
        }
        [Route("/accessdenied.html")]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Route("/logout.html")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        [HttpPost]
        [Route("/signin.html")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginFormModel user)
        {
            if (ModelState.IsValid)
            {
                ECoPartUser foundedUser = null;

                foundedUser = await userManager.FindByEmailAsync(user.Email);

                if (foundedUser == null)
                {
                    ViewBag.Message = "E-Poçtunuz və ya şifrəniz yanlışdır!";
                    goto end;
                }
                var signInResult = await signInManager.PasswordSignInAsync(foundedUser, user.Password, true, true);

                if (!signInResult.Succeeded)
                {
                    ViewBag.Message = "E-Poçtunuz və ya şifrəniz yanlışdır!";
                    goto end;
                }

                var callBackUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrWhiteSpace(callBackUrl))
                {
                    return Redirect(callBackUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            end:
            return View(user);
        }
        [AllowAnonymous]
        [Route("/register.html")]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("/register.html")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ECoPartUser();
                user.PhoneNumber = null;
                user.Email = model.Email;
                if (db.Users.Any(u=>u.PhoneNumber==model.PhoneNumber))
                {
                    ViewBag.Message = "Bu Nömrə ilə artıq qeydiyyatdan keçmisiniz";
                    goto end;
                }
                user.PhoneNumber = model.PhoneNumber;
                user.StoreName = model.StoreName;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.UserName = $"{model.Name}.{model.Surname}";

                user.PhoneNumberConfirmed = true;
                var result = await userManager.CreateAsync(user,model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string path = $"{ctx.GetAppLink()}/registration-confirm.html?email={user.Email}&token={token}";
                    var emailResponse = configuration.SendEmail(user.Email, "EcoPart istifadəçi qeydiyyatı", $"Zəhmət olmasa <a href=\"{path}\">link</a> vasitəsilə qeydiyyatı tamamlayasınız");
                    if (emailResponse)
                    {
                        ViewBag.Message = "Təbriklər qeydiyyat Tamamlandı, Sizinlə Tezliklə Əlaqə Saxlanılacaq";
                    }
                    else
                    {
                        ViewBag.Message = "E-mailə göndərərkən səhv aşkar olundu, yenidən cəhd edin";
                    }
                    
                    return RedirectToAction(nameof(SignIn));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            end:
            return View(model);
        }
        [HttpGet]
        [Route("/registration-confirm.html")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirm(string email,string token)
        {
            var foundedUser = await userManager.FindByEmailAsync(email);
            if (foundedUser==null)
            {
                ViewBag.Message = "Xətalı Token göndərilib";
                goto end;
            }
            token = token.Replace(" ", "+");
            var result = await userManager.ConfirmEmailAsync(foundedUser,token);
            if (!result.Succeeded)
            {
                ViewBag.Message = "Xətalı Token göndərilib";
                goto end;
            }
            ViewBag.Message = "Hesabınız təsdiqləndi, sizinlə tezliklə əlaqə saxlanılacaq";
        end:
            return RedirectToAction(nameof(SignIn));
        }
    }
}
