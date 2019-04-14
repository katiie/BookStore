using Bookstore.Data.IRepository;
using Bookstore.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Bookstore.Data.Repository
{
    public class BookRepository<TEntity> : IBookRepository<TEntity> where TEntity : class
    {
        protected readonly DatabaseContext db;
        public BookRepository(DatabaseContext db)
        {
            this.db = db;
        }
        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            db.Set<TEntity>().AddRange(entities);
            //throw new NotImplementedException();
        }

        public void Edit(TEntity entity)
        {
            db.Entry(entity).State = EntityState.Detached;
            db.Entry(entity).State = EntityState.Modified;
            // throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return db.Set<TEntity>().Where(predicate);
        }

        public TEntity FindByID(Func<TEntity, bool> predicate)
        {
            return db.Set<TEntity>().FirstOrDefault(predicate);
        }
        public TEntity Get(Guid id)
        {
            return db.Set<TEntity>().Find(id);
        }
        public IEnumerable<TEntity> GetAll(string[] includes)
        {
            return includes.Aggregate(db.Set<TEntity>().AsQueryable(), (query, path) => query.Include(path));
        }
       
        public IEnumerable<TEntity> GetAll()
        {
            return db.Set<TEntity>().ToList();
        }

        public void Remove(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
            //throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TEntity> entity)
        {
            db.Set<TEntity>().RemoveRange(entity);
        }

    }
}