using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Service.IReposervices;
using Bookstore.Service.Reposervices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Books.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class HomeController : Controller
    {
        private readonly IDashboardService _IDashboardService;

        public HomeController(IDashboardService IDashboardService)
        {
            _IDashboardService = IDashboardService;
        }
        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult Dashboard()
        {
            TempData["BookCount"] = _IDashboardService.BookCount();
            TempData["StoreCount"] = _IDashboardService.StoreCount();
            TempData["UserCount"] = _IDashboardService.UserCount();
            return View();

        }
        public IActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signout()
        {
            ViewBag.Message = "";
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SignIn(string password)
        {

            if (password == "admin")
            {
                
                    var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Json(new { Message = "Login in was Succesfully", status = "success", Success = true });


            }
            return Json(new { Message = "Invalid Credentials", status = "error", Success = false });
        }


    }
}