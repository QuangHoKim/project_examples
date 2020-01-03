using AutoMapper;
using Vnn88.Common.Infrastructure;
using Vnn88.DataAccess.Models;
using Vnn88.DataModel;

namespace Vnn88.Web.Infrastructure.Mapper
{
    /// <inheritdoc />
    /// <summary>
    /// Create mapping for account
    /// </summary>
    public class AccountMapping : Profile
    {
        /// <summary>
        /// Ctor for mapping account
        /// </summary>
        public AccountMapping()
        {
            CreateMap<LoginModel, LoginModel>()
                .ForMember(dest => dest.Pass,
                    opt =>
                    {
                        opt.Condition(m => !string.IsNullOrEmpty(m.UserName));
                        opt.MapFrom(src => Encryptor.CalculateHash(src.UserName));
                    });
            CreateMap<ChangePasswordModel, Users>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Password,
                    opt =>
                    {
                        opt.Condition(m => !string.IsNullOrEmpty(m.Password));
                        opt.MapFrom(src => Encryptor.CalculateHash(src.Password));
                    });
        }
    }
}
