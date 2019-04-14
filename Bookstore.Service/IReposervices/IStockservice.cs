using Bookstore.Core.Models;
using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Service.IReposervices
{
    public interface IStockService
    {
        Guid Add(Stock Stock);
        Guid AddOrEdit(Stock Stock);
        bool Edit(Stock Stock);
        Stock Get(Guid id);
        bool Delete(Guid id);
        IEnumerable<Stock> Gets();
        IEnumerable<Stock> GetByBookId(Guid Id);

        IEnumerable<Stock> CompareLocationByRequestDistance( GeoCoordinate saddress,Guid Id);


        string AddOrEditWithReturn(Stock Stock);
    }
}
