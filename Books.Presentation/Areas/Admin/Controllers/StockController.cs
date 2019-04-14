using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Service.IReposervices;
using Bookstore.Core.Models;

namespace Books.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StockController : Controller
    {
        private readonly IStockService _IStockservice;

        public StockController(IStockService IStockservice)
        {
            _IStockservice = IStockservice;
        }

        public ActionResult AllStocks(Guid id)
        {
            var query =  _IStockservice.GetByBookId(id).ToList();
            return new JsonResult(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Stock collection)
        {
            try
            {
                if(collection.Quantity < collection.Dispatched && collection.Dispatched != 0 && collection.Id != Guid.Empty)
                {
                    return Json(new { message = "Quantity should be greater than item dispatched", status = false });
                }
                // TODO: Add insert logic here
                var res= _IStockservice.AddOrEditWithReturn(collection);
                return Json(new {message= res== string.Empty? "Stock added succesfully" : res, status= res == string.Empty ? true:false});
            }
            catch
            {
                return Json(new { message = "An error occured", status = false });
               
            }
        }

       
        // POST: Stock/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                // TODO: Add insert logic here
                _IStockservice.Delete(id);
                return Json(new { message = "Stock removed succesfully", status = true });
            }
            catch
            {
                return Json(new { message = "An error occured" });

            }
        }
    }
}