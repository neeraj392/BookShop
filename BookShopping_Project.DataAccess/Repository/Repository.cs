using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace BookShopping_Project.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbset = _context.Set<T>();
        }
        public void add(T entity)
        {
            dbset.Add(entity);
        }

        public T FirstorDefault(Expression<Func<T, bool>> filter = null, string includeproperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
                query = query.Where(filter);
            if(includeproperties!=null)
            { 
                foreach (var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return query.FirstOrDefault();
        }

        public T Get(int id)
        {
            return dbset.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includepropertities = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            query = query.Where(filter);    
            if (includepropertities != null)
            {
                foreach (var inludeprop in includepropertities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(inludeprop);
                }
                if (orderby != null)
                return orderby(query).ToList();
            }          
             return query.ToList();
        }

        public void Remove(int id)
        {
            T entity = dbset.Find(id);
            dbset.Remove(entity);
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
    }
}
