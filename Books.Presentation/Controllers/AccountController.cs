using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Service.IReposervices;
using Bookstore.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Books.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDashboardService _IDashboardService;

        public AccountController(IDashboardService IDashboardService)
        {
            _IDashboardService = IDashboardService;
        }
        [Authorize]
        public IActionResult Dashboard()
        {

            TempData["BookCount"] = _IDashboardService.BookCount();
            TempData["storeCount"] = _IDashboardService.StoreCount();

            return View();
        }
        public IActionResult Register()
        {
            ViewBag.Message = null;
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (_IDashboardService.GetByEmail(user.Email) == null)
                {
                    var result = _IDashboardService.AddUser(user);
                     message = result != Guid.Empty? "Proceed to Login": "The Email has been taken";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public IActionResult Signout()
        {
            ViewBag.Message = "";
            Remove("Uid");
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public IActionResult SignIn(User user)
        {

            if (user.Email != string.Empty)
            {
                user = _IDashboardService.GetByEmail(user.Email);
                if ( user.Id != Guid.Empty)
                {
                    var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Email)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    Set("Uid", user.Id.ToString(), 1000);
                    return RedirectToAction("Dashboard");
                }
            }
            ViewBag.Message = "Invalid Credentials";
            return View();
        }


        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard");
            ViewBag.Message = "";
            return View();
        }

        public string Get(string key)
        {
            return Request.Cookies[key];
        }
        /// <summary>  
        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
        /// <param name="key">Key</param>  
        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}