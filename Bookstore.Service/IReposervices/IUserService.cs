using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Core.Models;
using Bookstore.Data.UnitOfWork;


namespace Bookstore.Service.IReposervices
{
    public interface IUserService
    {
        Guid AddUser(User User);
        Guid AddOrEditUser(User User);
        bool EditUser(User User);
        User GetUser(Guid id);
        bool RemoveUser(Guid id);
        IEnumerable<User> GetAllUser();
        User GetByEmail(string key);

    }
}
