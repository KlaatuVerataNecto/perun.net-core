using Microsoft.EntityFrameworkCore;
using domain.model.common;

namespace persistance.ef.common
{
    public interface IRepository<T> where T : Entity
    {
        T Add(T item);
        void Delete(T entity);
        T Update(T item, object key);
        T Get(int id);
    }
}
