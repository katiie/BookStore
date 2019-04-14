
using Bookstore.Data.IRepository;
using Bookstore.Data.Model;
using Bookstore.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookstore.Data.UnitOfWork
{
    public class UnitOfWork
    {

        private readonly DatabaseContext _db = new DatabaseContext();
        public IBookRepository<T> RepositoryFor<T>() where T : class
        {
            return new BookRepository<T>(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}