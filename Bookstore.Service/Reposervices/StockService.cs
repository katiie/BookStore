using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Bookstore.Core.Models;
using Bookstore.Service.IReposervices;
using Bookstore.Data.UnitOfWork;

namespace Bookstore.Service.Reposervices
{
    public class StockService : IStockService
    {
        private UnitOfWork uow = new UnitOfWork();
      
        public string AddOrEditWithReturn(Stock Stock)
        {
            var item = uow.RepositoryFor<Stock>().Find(d => d.StoreId == Stock.StoreId && d.BookId == Stock.BookId);

            if (Stock.Id == Guid.Empty)
            {
                if (item == null || item.Count() > 0 )
                    return "Location has been added";
                else
                    Add(Stock);

                return string.Empty;
            }
            else
            {
                //check if quantity has changed and if chage should be allowed
                //have 10 , dispatched all 10 new quantity shukd be greater than 10
                if (Stock.Quantity < item.FirstOrDefault().Quantity)
                    return "Quantity should be greater than item dispatched";
                Edit(Stock);
                return string.Empty;
            }
        }
        public Guid Add(Stock Stock)
        {
            try
            {
                uow.RepositoryFor<Stock>().Add(Stock);
                uow.Save();
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
            return Stock.Id;
        }

        public Guid AddOrEdit(Stock Stock)
        {
            if (Stock.Id == Guid.Empty)
                return Add(Stock);
            else
                if (Edit(Stock))
                return Stock.Id;
            else
                return Guid.Empty;
        }

        public bool Edit(Stock Stock)
        {
            try
            {
                uow.RepositoryFor<Stock>().Edit(Stock);
                uow.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        public IEnumerable<Stock> Gets()
        {
            return uow.RepositoryFor<Stock>().GetAll();
        }

        public IEnumerable<Stock> GetByBookId(Guid Id)
        {
            return uow.RepositoryFor<Stock>().Find(d => d.BookId == Id); 
        }

        public Stock Get(Guid id)
        {
            return uow.RepositoryFor<Stock>().Get(id);
        }
       
        public bool Delete(Guid id)
        {
            try
            {
                var item = Get(id);
                uow.RepositoryFor<Stock>().Remove(item);
                uow.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<Stock> CompareLocationByRequestDistance( GeoCoordinate saddress, Guid Id)
        {
            var stocks = uow.RepositoryFor<Stock>().Find(d => d.BookId == Id);
            if (stocks != null)
            {
                foreach (var stock in stocks)
                {
                    var eCoord = new GeoCoordinate(stock.Store.Latitude, stock.Store.Longitude);
                    var distance = saddress.GetDistanceTo(eCoord);
                    stock.Distance = distance;
                }
            }
            return stocks != null ? stocks.OrderByDescending(d => d.Distance) : stocks;
        }
    }
}
