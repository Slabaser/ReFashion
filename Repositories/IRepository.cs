using System.Collections.Generic;
using System.Linq.Expressions;

namespace ECommerceApp.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(string id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(string id, T entity);
        void Remove(string id);
    }
}
