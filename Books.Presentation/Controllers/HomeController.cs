using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Books.Presentation.Models;
using Bookstore.Service.IReposervices;
using Bookstore.Core.Models;
using Microsoft.AspNetCore.Http;
using Books.Presentation.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Authorization;

namespace Books.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userServices;
        private readonly IStoreService _IStoreService;
        private readonly IStockService _IStockservice;
        private readonly IBookService _IBookServices;
        public HomeController(IStoreService IStoreService, IUserService IUserServices, IBookService IBookServices, IStockService IStockservice)
        {
            _IStoreService = IStoreService;
            _userServices = IUserServices;
            _IStockservice = IStockservice;
            _IBookServices = IBookServices;
        }

        

        public IActionResult Users()
        {
            return View();
        }
        
        public ActionResult BookDetails(Guid id)
        {
            var book = _IBookServices.Get(id);
          
            TempData["Bid"] = id;
            return View(book);
        }

       public ActionResult Stores()
        {
            return View();
        }
        public ActionResult AllBooks()
        {
            var query = (from e in _IBookServices.Gets().OrderBy(s => s.Name)
                         select new
                         {
                             Name = e.Name,
                             DateCreated = e.DateModified.ToLongDateString(),
                             Description = e.Description,
                             ISBN = e.ISBN,
                             Id = e.Id,
                             Author = e.Author,
                             LocationTag = e.LocationTag

                         }).ToList();
            return new JsonResult(query);
        }
        public ActionResult AllStores()
        {
            var query = (from e in _IStoreService.Gets()
                         select new
                         {
                             lat = e.Latitude.ToString(),
                             lng = e.Longitude.ToString(),
                             Address = e.Address,
                             Name = e.Name == string.Empty? e.Address: e.Name

                         }).ToList();
            return new JsonResult( query);
        }
        public ActionResult AllStocks(Guid? sid)
        {
            Guid Id = sid.Value;
            var query = from e in _IStockservice.GetByBookId(Id)
                        select new
                        {
                            Quantity = e.Quantity,
                            Location = e.Store.Address,
                            Dispatched = e.Dispatched,
                            LeftWith = e.Quantity - e.Dispatched,
                            Id = e.Id,
                            StoreId = e.StoreId

                        };
            return new JsonResult(query);
        }

        public IEnumerable<Stock> sortLocationByDistance(IEnumerable<Stock> stocks, GeoCoordinate saddress)
        {

            if(stocks != null)
            {
                foreach(var stock in stocks)
                {
                    var eCoord = new GeoCoordinate(stock.Store.Latitude, stock.Store.Longitude);
                    var distance  = saddress.GetDistanceTo(eCoord);
                    stock.Distance = distance;
                }
            }
            return stocks;
        }
       
        public IActionResult Books()
        {
            return View(_IBookServices.Gets().OrderBy(s => s.Name));
        }
        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
      
    }
}
