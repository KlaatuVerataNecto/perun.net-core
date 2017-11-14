using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;

using domain.model.common;

namespace persistance.ef.common
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private IEFContext _dbContext;

        public Repository(IEFContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T Get(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return _dbContext.Set<T>().SingleOrDefault(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _dbContext.Set<T>().Where(match).ToList();
        }

        public int Count()
        {
            return _dbContext.Set<T>().Count();
        }

        public T Add(T item)
        {
            _dbContext.Set<T>().Add(item);
            _dbContext.SaveChanges();
            return item;
        }

        public T Update(T t, object key)
        {
            if (t == null)
                return null;
            T exist = _dbContext.Set<T>().Find(key);
            if (exist != null)
            {
                _dbContext.Entry(exist).CurrentValues.SetValues(t);
                _dbContext.SaveChanges();
            }
            return exist;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

    }
}
