using Vnn88.DataAccess.Models;

namespace Vnn88.Repository
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork
    {
        IUsersRepository UsersRepository { get; }
        AppDbContext AppDbContext { get; }
        void Save();
    }
}