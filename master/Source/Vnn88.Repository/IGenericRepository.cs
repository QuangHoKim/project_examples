using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Vnn88.Repository
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        // Marks an entity as new
        void Add(T entity);
        // Marks an entity as modified
        void Update(T entity);
        // Marks an entity to be removed
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void DeleteRange(IEnumerable<T> entities);
        void DeleteRange(params T[] entities);
        // Get an entity by int id
        T GetById(int id);
        // Get an entity using delegate
        T Get(Expression<Func<T, bool>> where);
        // Gets all entities of type T
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        // Gets entities using delegate
        IEnumerable<T> GetMany(Func<T, bool> where);
        IQueryable<T> ObjectContext { get; set; }
    }
}