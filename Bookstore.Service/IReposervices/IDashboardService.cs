using Bookstore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Service.IReposervices
{
    public interface IDashboardService
    {
        int BookCount();
        int StoreCount();
        int UserCount();

        User GetByEmail(string email);
        Guid AddUser(User user);
    }
}
