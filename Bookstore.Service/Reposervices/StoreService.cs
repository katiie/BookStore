using Bookstore.Service.IReposervices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Core.Models;
using Bookstore.Data.UnitOfWork;

namespace Bookstore.Service.Reposervices
{
    public class StoreService : IStoreService
    {
        private UnitOfWork uow = new UnitOfWork();
        public Guid Add(Store Store)
        {
            try
            {
                uow.RepositoryFor<Store>().Add(Store);
                uow.Save();
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
            return Store.Id;
        }

        public string AddOrEdit(Store Store)
        {
            string result = string.Empty;
            var loc = GetStoreByAddress(Store.Address);
            if (Store.Id == Guid.Empty && loc == null)
                Add(Store);
            else if (Store.Id == Guid.Empty && loc != null)
                result = "Store has been added";
            else if (loc != null && loc.Id != Store.Id)
                result = "Store has been added";
            else
            {
              var editRes=  Edit(Store);
                result = editRes ? "Store has been added" : string.Empty;
            }

            return result;
        }
        public Store GetStoreByAddress(string key)
        {
            var result = uow.RepositoryFor<Store>().Find(d => d.Address == key);
            return result.Count() > 0 ? result.FirstOrDefault() : null;
        }
        public bool Edit(Store Store)
        {
            try
            {
                Store.DateModified = DateTime.UtcNow;
                uow.RepositoryFor<Store>().Edit(Store);
                uow.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<Store> Gets()
        {
            return uow.RepositoryFor<Store>().GetAll();
        }

        public Store Get(Guid id)
        {
            return uow.RepositoryFor<Store>().Get(id);
        }

        public bool Delete(Guid id)
        {
           
            try
            {
                var _getStore = Get(id);
                uow.RepositoryFor<Store>().Remove(_getStore);
                uow.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

      
    }
}
