using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Service.IReposervices;
using Bookstore.Core.Models;

namespace Locations.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreController : Controller
    {
        private readonly IStoreService _IStoreServices;
        private readonly IStockService _IStockservice;

        public StoreController(IStoreService ILocationServices, IStockService IStockservice)
        {
            _IStoreServices = ILocationServices;
            _IStockservice = IStockservice;
        }
        // GET: Location
        public ActionResult Index()
        {
            return View(_IStoreServices.Gets());
        }
        public JsonResult AllStoreLocations()
        {
            var query = (from e in _IStoreServices.Gets().OrderBy(s => s.State)
                         select new
                         {
                             State = e.State,
                             Address = e.Address,
                             Id = e.Id,
                             Name= e.Name,
                             Lat = e.Latitude,
                             Lng=e.Longitude
                            
                         }).ToList();
            return new JsonResult(query);
        }
        // GET: Location/Details/5
        public ActionResult Details(Guid id)
        {
            var Location = _IStoreServices.Get(id);
           
            if (Location != null)
            {
                return View(Location);
            }
            else
            {
                return NoContent();
            }
           
        }

     
        // POST: Location/Create
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Create(Store collection)
        {
            try
            {
                if(collection.State == null || collection.Address == null)
                    return Json(new { Message = "Input is required", status = "error", Success = false });

                // TODO: Add insert logic here
                if (collection.Id == Guid.Empty)
                {
                    var result = _IStoreServices.Add(collection);
                    return Json(result == Guid.Empty ? new { Message = "An error Occured ;Ensure the store has not been previously added", status = "error", Success = false } : new { Message = "Record Added Succesfully", status = "success", Success = true });

                }
                else
                {
                    var result = _IStoreServices.Edit(collection);
                    return Json(!result ? new { Message = "An error Occured ;", status = "error", Success = false } : new { Message = "Record Updated Succesfully", status = "success", Success = true });

                }
                
            }
            catch
            {
                return Json(new { Message = "An error occured", status = "success", Success = true });
            }
        }

      
        // POST: Locations/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                _IStoreServices.Delete(Id);
                ViewBag.ActionStatus = "Update Successful";
                return Json(new { Message = "Record Deleted Succesfully", status = "success", Success = true });

            }
            catch (Exception e)
            {
                return Json(new { Message = "An error Occured ;", status = "error", Success = false });

            }
        }

     
    }
}