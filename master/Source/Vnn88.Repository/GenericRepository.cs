using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vnn88.Common.Infrastructure;
using Vnn88.DataAccess.Models;

namespace Vnn88.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Generic repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Properties
        private readonly Type _type;
        private readonly HttpContext _httpContext;
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        #endregion

        /// <summary>
        /// GenericRepository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="contextAccessor"></param>
        public GenericRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _httpContext = contextAccessor.HttpContext;
            _type = typeof(T);
            ObjectContext = _dbSet;
        }

        public IQueryable<T> ObjectContext { get; set; }
        #region Implementation
        /// <summary>
        /// Add T entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            SetCreated(entity);
            SetUpdated(entity);
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Update T entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            SetUpdated(entity);
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete T entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete a list entities
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Delete a list entities
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRange(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }
        /// <summary>
        /// Delete T entity by condition
        /// </summary>
        /// <param name="where"></param>
        public void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where(where).AsEnumerable();
            foreach (T obj in objects)
                _dbSet.Remove(obj);
        }

        /// <summary>
        /// Get T entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Get all entities by some condition
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Get many T entity by condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IEnumerable<T> GetMany(Func<T, bool> where)
        {
            return _dbSet.AsEnumerable().Where(where);
        }

        /// <summary>
        /// Get T entity by condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).FirstOrDefault();
        }

        #endregion

        #region Method to check common fields existed
        /// <summary>
        /// Check model have a specific property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected bool HasProperty(string property)
        {
            return _type.GetProperty(property) != null;
        }
        protected void SetProperty(T entity, string property, object value)
        {
            if(value != null)
            {
                entity.GetType().GetProperty(property).SetValue(entity,
                    int.TryParse(value.ToString(), out var number) ? number : value);
            }
        }
        protected void SetCreated(T entity)
        {
            if (HasProperty(Constants.CommonFields.CreatedBy))
            {
                var accountId = _httpContext.User.Claims
                    .FirstOrDefault(m => m.Type == Constants.ClaimName.AccountId)?.Value;

                if(!string.IsNullOrEmpty(accountId))
                {
                    SetProperty(entity, Constants.CommonFields.CreatedBy, accountId);
                }
            }
            if (HasProperty(Constants.CommonFields.CreatedOn))
            {
                SetProperty(entity, Constants.CommonFields.CreatedOn, DateTime.Now);
            }
        }
        protected void SetUpdated(T entity) 
        {
            if (HasProperty(Constants.CommonFields.UpdatedBy))
            {
                var accountId = _httpContext.User.Claims
                    .FirstOrDefault(m => m.Type == Constants.ClaimName.AccountId)?.Value;

                if (!string.IsNullOrEmpty(accountId))
                {
                    SetProperty(entity, Constants.CommonFields.UpdatedBy, accountId);
                }
            }
            if (HasProperty(Constants.CommonFields.UpdatedOn))
            {
                SetProperty(entity, Constants.CommonFields.UpdatedOn, DateTime.Now);
            }
        }
        #endregion
    }
}