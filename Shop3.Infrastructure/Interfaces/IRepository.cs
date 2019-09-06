using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shop3.Infrastructure.Interfaces
{
    public interface IRepository<T, K> where T : class // dữ liệu kiểu T với key là K với T là 1 class
    {
        // params truyền vào 1 danh sách các tham số
        // Expression truyền vào 1 biểu thức trong biểu thức truyền vào 1 function : kiểu là T và out ra 1 object
        T FindById(K id, params Expression<Func<T, object>>[] includeProperties);

        // bool true thì mới lấy về false thì khỏi :v
        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        // void Update(T entity);

        void Update(K id, T entity, params Expression<Func<T, object>>[] updatedProperties);

        void Remove(T entity);

        void RemoveById(K id);

        void RemoveMultiple(List<T> entities);

        void RemoveMultipleWithListKey(List<K> keyEntities);

        Task<IQueryable<T>> FindSingleAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetAllAsync(Expression<Func<T, bool>> predicate);
    }
}
