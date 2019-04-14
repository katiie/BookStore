using Bookstore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Service.IReposervices
{
     public interface IStoreService
    {
        Guid Add(Store Location);
        string AddOrEdit(Store Location);
        bool Edit(Store Location);
        Store Get(Guid id);

        Store GetStoreByAddress(string key);

        bool Delete(Guid id);
        IEnumerable<Store> Gets();

    }
}
