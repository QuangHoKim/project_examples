using FluentValidation;
using Vnn88.DataModel;

namespace Vnn88.Web.Infrastructure.Validation
{
    public class ChangePasswordValidation : AbstractValidator<LoginModel>
    {
        //public ChangePasswordValidation()
        //{
        //    RuleFor(reg => reg.Password).NotNull()
        //        .WithMessage(MessageResource.NullPassword)
        //        .MinimumLength(6)
        //        .WithMessage(MessageResource.PasswordLength)
        //        .MaximumLength(50);
        //    RuleFor(reg => reg.ConfirmPassword).NotNull()
        //        .WithMessage(MessageResource.NullPassword)
        //        .MinimumLength(6)
        //        .WithMessage(MessageResource.PasswordLength)
        //        .MaximumLength(50)
        //        .When(reg => reg.Id == 0)
        //        .Equal(reg => reg.Password).WithMessage(MessageResource.Compare);
           
        //}
    }
}
