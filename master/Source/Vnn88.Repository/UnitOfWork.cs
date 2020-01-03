using Microsoft.AspNetCore.Http;
using Vnn88.DataAccess.Models;

namespace Vnn88.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Unit of work class
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region inject field variables
        private readonly  AppDbContext _appDbContext;
        private IUsersRepository _usersRepository;
        #endregion

        #region data members
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        /// <summary>
        /// Unit of work constructor
        /// </summary>
        /// <param name="tapDoorCloudDbContext"></param>
        /// <param name="contextAccessor"></param>
        public UnitOfWork(AppDbContext tapDoorCloudDbContext, IHttpContextAccessor contextAccessor)
        {
            _appDbContext = tapDoorCloudDbContext;
            _httpContext = contextAccessor;
        }

        #region Properties
        /// <summary>
        /// Get AppDbContext
        /// </summary>
        public AppDbContext AppDbContext => _appDbContext;
        public IUsersRepository UsersRepository
        {
            get
            {
                return _usersRepository =
                    _usersRepository ?? new UsersRepository(_appDbContext, _httpContext);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Save
        /// </summary>
        public void Save()
        {
            _appDbContext.SaveChanges();
        }
        #endregion
    }
}