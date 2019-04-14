using Bookstore.Core.Models;
using Bookstore.Data.UnitOfWork;
using Bookstore.Service.IReposervices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Service.Reposervices
{
    public class DashboardService : IDashboardService
    {
        private UnitOfWork uow = new UnitOfWork();
        private UserService _userService = new UserService();

        public Guid AddUser(User user )
        {
            return _userService.AddUser(user);
        }

        

        public int BookCount()
        {
            var list = uow.RepositoryFor<Book>().GetAll();
            return list != null ? list.Count() : 0;
        }

        public User GetByEmail(string email)
        {
            return _userService.GetByEmail(email);
        }

        
        public int StoreCount()
        {
            var list = uow.RepositoryFor<Store>().GetAll();
            return list != null ? list.Count() : 0;
        }

        public int UserCount()
        {
            return _userService.GetCounts();
        }
    }
}
