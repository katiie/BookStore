using Bookstore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Service.IReposervices
{
    public interface IBookService
    {
        Guid Add(Book Book);
        Guid AddOrEdit(Book Book);
        bool Edit(Book Book);
        Book Get(Guid id);
        bool Delete(Guid id);
        IEnumerable<Book> Gets();
        int GetCounts();
       
    }
}
