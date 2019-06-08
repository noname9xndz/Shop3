using Microsoft.EntityFrameworkCore;
using Shop3.Infrastructure.Interfaces;
using Shop3.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Shop3.Data.Interfaces;
using Shop3.Utilities.Extensions;

namespace Shop3.Data.EF
{
    // chỉ đối tượng nào kế thừa từ domainEntity có thể sử dụng các phương thức thức trong EFRepository 
    // truy vấn nào chỉ đọc(ko dùng cho thêm sửa xóa) có thể dùng .AsNoTracking() để tăng tốc độ
    public class EFRepository<T, K> : IRepository<T, K>, IDisposable where T : DomainEntity<K>
    {
        private readonly AppDbContext _context;

        public EFRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public T FindById(K id, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(x => x.Id.Equals(id));
        }

        public T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Remove(K id)
        {
            var entity = FindById(id);
            Remove(entity);
        }

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        //public void Update(T entity)
        //{
        //    _context.Set<T>().Update(entity);
        //}
        public virtual void Update(K id, T entity, params Expression<Func<T, object>>[] updatedProperties)
        {//chỉ update cái gì thay đổi
            var dbEntity = _context.Set<T>().AsNoTracking().Single(p => p.Id.Equals(id));
            var databaseEntry = _context.Entry(dbEntity);
            var inputEntry = _context.Entry(entity);
            if (updatedProperties.Any())
            {
                //update explicitly mentioned properties
                foreach (var property in updatedProperties)
                {
                    databaseEntry.Property(property).IsModified = true;
                    PropertyInfo prop = property.GetPropertyAccess();
                    databaseEntry.Property(property).IsModified = true;
                    databaseEntry.Property(property).CurrentValue = prop.GetValue(entity, null);
                }
            }
            else
            {
                //no items mentioned, so find out the updated entries
                //IEnumerable<string> dateProperties = typeof(IDateTracking).GetProperties().Select(x => x.Name);
                //IEnumerable<string> deleteProperties = typeof(IHasSoftDelete).GetProperties().Select(x => x.Name);
                //IEnumerable<string> domainProperties = typeof(DomainEntity<K>).GetProperties().Select(x => x.Name);
                IEnumerable<string> dateProperties = typeof(IDateTracking).GetPublicProperties().Select(x => x.Name);
                IEnumerable<string> deleteProperties = typeof(IHasSoftDelete).GetPublicProperties().Select(x => x.Name);
                IEnumerable<string> domainProperties = typeof(DomainEntity<K>).GetPublicProperties().Select(x => x.Name);

                var allProperties = databaseEntry.Metadata.GetProperties()
                    .Where(x => !dateProperties.Contains(x.Name))
                    .Where(x => !deleteProperties.Contains(x.Name))
                    .Where(x => !domainProperties.Contains(x.Name));

                foreach (var property in allProperties)
                {
                    var proposedValue = inputEntry.Property(property.Name).CurrentValue;
                    var originalValue = databaseEntry.Property(property.Name).OriginalValue;

                    if ((proposedValue != null && !proposedValue.Equals(originalValue))
                        || (property.PropertyInfo.PropertyType.IsGenericType
                        && property.PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        databaseEntry.Property(property.Name).IsModified = true;
                        databaseEntry.Property(property.Name).CurrentValue = proposedValue;
                    }

                }
            }
            _context.Set<T>().Update(dbEntity);
        }
    }
}
