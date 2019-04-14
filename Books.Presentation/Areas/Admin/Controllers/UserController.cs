using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Service.IReposervices;

namespace Books.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userServices;


        public UserController(IUserService IUserServices)
        {
            _userServices = IUserServices;
        }
        // GET: User
        public ActionResult Index()
        {
            var users = _userServices.GetAllUser();
            return View(users);
        }

        // GET: User/Details/5
        public ActionResult UserDetails(int id)
        {
            return View();
        }

        public ActionResult BookRequest()
        {
            return View();
        }

        // GET: User/Create
       
    }
}