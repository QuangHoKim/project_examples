using System.Linq;
using AutoMapper;
using Vnn88.Common.Infrastructure;
using Vnn88.DataAccess.Models;
using Vnn88.DataModel;
using Vnn88.Repository;

namespace Vnn88.Service
{
    /// <summary>
    /// Interface User Service
    /// </summary>
    public interface IUsersService
    {
        Users Login(LoginModel model);
        bool ChangePass(ChangePasswordModel changePassword);
        void CreateUser(Users model);
    }
    /// <inheritdoc />
    /// <summary>
    /// User Service
    /// </summary>
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Service Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Users Login(LoginModel model)
        {
            var account =
                _unitOfWork.UsersRepository.ObjectContext.FirstOrDefault(m => m.UserName.Equals(model.UserName));

            if (account != null)
            {
                if (Encryptor.CheckMatch(account.Password, model.Pass))
                {
                    return account;
                }
            }
            return null;
        }
        /// <summary>
        /// Change pass
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        public bool ChangePass(ChangePasswordModel changePassword)
        {
            var user = _unitOfWork.UsersRepository.ObjectContext.FirstOrDefault(m => m.Id == changePassword.Id);
            if (user == null) return false;
            Mapper.Map(changePassword, user);
            _unitOfWork.UsersRepository.Update(user);
            _unitOfWork.Save();
            return true;
        }

        public void CreateUser(Users model)
        {
            _unitOfWork.UsersRepository.Add(model);
            _unitOfWork.Save();
        }
    }
}
