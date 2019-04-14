using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.Data.IRepository
{
    public interface IBookRepository<TEntity> where TEntity : class
    {
      
            TEntity Get(Guid id);
            IEnumerable<TEntity> GetAll();


            IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
            TEntity FindByID(Func<TEntity, bool> predicate);
            void Add(TEntity entity);
            void AddRange(IEnumerable<TEntity> entities);
            void Edit(TEntity entity);

            IEnumerable<TEntity> GetAll(string[] includes);

            void Remove(TEntity entity);
            void RemoveRange(IEnumerable<TEntity> entity);
        
    }
}
