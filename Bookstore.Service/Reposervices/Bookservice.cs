using Bookstore.Service.IReposervices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Core.Models;
using Bookstore.Data.UnitOfWork;

namespace Bookstore.Service.Reposervices
{
    public class BookService : IBookService
    {
        private UnitOfWork uow = new UnitOfWork();

        public Guid Add(Book Book)
        {
            try
            {
                var item = uow.RepositoryFor<Book>().Find(d=> d.ISBN == Book.ISBN);
                if (item == null || item.Count() > 1)
                    return Guid.Empty;

                uow.RepositoryFor<Book>().Add(Book);
                uow.Save();
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
            return Book.Id;
        }

        public Guid AddOrEdit(Book Book)
        {
            if (Book.Id == Guid.Empty)
                return Add(Book);
            else
                if (Edit(Book))
                return Book.Id;
            else
                return Guid.Empty;
        }

        public bool Edit(Book Book)
        {
            try
            {
                var item = uow.RepositoryFor<Book>().Find(d => d.ISBN == Book.ISBN);
                if (item == null || item.Count() == 0 || !item.Any(s=> s.Id ==Book.Id))
                    return false;

                uow.RepositoryFor<Book>().Edit(Book);
                uow.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<Book> Gets()
        {
            return uow.RepositoryFor<Book>().GetAll();
        }



        public Book Get(Guid id)
        {
            return uow.RepositoryFor<Book>().Get(id);
        }


        public bool Delete(Guid id)
        {
            try
            {
                var _getBook = Get(id);
                var _stock = uow.RepositoryFor<Stock>().Find(d => d.Id == id);
                uow.RepositoryFor<Stock>().RemoveRange(_stock);
                uow.RepositoryFor<Book>().Remove(_getBook);
                uow.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;

        }

        public int GetCounts()
        {
            return Gets().Count();
        }
    }
}
