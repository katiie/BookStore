using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Service.IReposervices;
using Bookstore.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Books.Presentation.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IBookService _IBookServices;
        private readonly IStoreService _IStoreService;
        private readonly IStockService _IStockservice;

        public BookController(IBookService IBookServices, IStockService IStockservice, IStoreService ILocationService)
        {
            _IStoreService = ILocationService;
            _IBookServices = IBookServices;
            _IStockservice = IStockservice;
        }
        // GET: Book
        public ActionResult Index()
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
                             Author = e.Author
                         }).ToList();
            return new JsonResult(query);
        }
        // GET: Book/Details/5
        public ActionResult Details(Guid id)
        {
            var book = _IBookServices.Get(id);
            ViewBag.StoreId = new SelectList(_IStoreService.Gets(), "Id", "Address");
            TempData["Bid"] = id;
            return View(book);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book collection)
        {
            try
            {
                // TODO: Add insert logic here
                _IBookServices.Add(collection);
                return Json(new { Message = "Record Added Succesfully", status = "success", Success = true });
            }
            catch
            {
                return Json(new { Message = "An error occured", status = "success", Success = true });
            }
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (collection.Id == Guid.Empty)
                {
                    var result = _IBookServices.Add(collection);
                    return Json(result == Guid.Empty ? new { Message = "An error Occured ;", status = "error", Success = false } : new { Message = "Record Added Succesfully", status = "success", Success = true });

                }
                else
                {
                    var result =_IBookServices.Edit(collection);
                    return Json(!result ? new { Message = "An error Occured ;", status = "error", Success = false } : new { Message = "Record Added Succesfully", status = "success", Success = true });

                }
            }
            catch
            {
                return Json(new { Message = "Record Updated Succesfully", status = "success", Success = true });
            }
        }

        // POST: Books/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            try
            {
                _IBookServices.Delete(Id);
                ViewBag.ActionStatus = "Update Successful";
                return Json(new { Message = "Record Deleted Succesfully", status = "success", Success = true });

            }
            catch (Exception e)
            {
                return Json(new { Message = "An error Occured ;", status = "error", Success = false });

            }
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
                            Name = e.Store.Name == null || e.Store.Name == string.Empty? "NA": e.Store.Name,
                            Id = e.Id,
                            StoreId = e.StoreId

                        };
            return new JsonResult(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStock(Stock collection)
        {
            try
            {
               
                // TODO: Add insert logic here
                var res = _IStockservice.AddOrEditWithReturn(collection);
                return Json(new { message = res == string.Empty ? "Stock updated succesfully" : res, status = res == string.Empty ? true : false });
            }
            catch(Exception e)
            {
                return Json(new { message = "An error occured", status = false });

            }
        }


        // POST: Stock/Delete/5
        [HttpPost]
        public ActionResult DeleteStock(Guid id)
        {
            TempData["Bid"] = id;
            try
            {
                var result = _IStockservice.Delete(id);
                return Json(new { message = "Stock removed succesfully", status = true });
            }
            catch
            {
                return Json(new { message = "An error occured", status = false });
            }
        }
    }
}