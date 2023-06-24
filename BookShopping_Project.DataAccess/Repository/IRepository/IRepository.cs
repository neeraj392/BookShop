using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface IRepository <T> where T:class
    {
        T Get(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string includepropertities = null    //for joins
            );
        T FirstorDefault(
            Expression<Func<T, bool>> filter = null,
            string includeproperties = null
            );
        void add(T entity);
        void Remove(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
