using Microsoft.AspNetCore.Http;
using Vnn88.DataAccess.Models;

namespace Vnn88.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Interface for UserRepository
    /// </summary>
    public interface IUsersRepository : IGenericRepository<Users>
    {
    }
    public class UsersRepository : GenericRepository<Users>, IUsersRepository
    {
        /// <inheritdoc />
        /// <summary>
        /// UserRepository
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="contextAccessor"></param>
        public UsersRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
        }
    }
}
