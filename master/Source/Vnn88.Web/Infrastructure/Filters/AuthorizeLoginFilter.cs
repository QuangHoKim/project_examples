using Microsoft.AspNetCore.Mvc.Filters;

namespace Vnn88.Web.Infrastructure.Filters
{
    /// <summary>
    /// ValidateRequestFilter class
    /// </summary>
    public class AuthorizeLoginFilter : ActionFilterAttribute
    {
        //private readonly IAccountService _accountService;
        //private readonly HttpContext _httpContext;

        //public AuthorizeLoginFilter(IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        //{
        //    _accountService = accountService;
        //    _httpContext = httpContextAccessor.HttpContext;
        //}
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    var accountType = _httpContext.User.GetAccountType();
        //    var accountEmail = _httpContext.User.Identity.Name;
        //    var account = _accountService.GetById(_httpContext.User.GetAccountId());
        //    if(!_httpContext.User.Identity.IsAuthenticated || account.Status != (short)Status.Valid
        //        || account.Company.Status != (short)Status.Valid || accountType != account.Type || accountEmail != account.Username)
        //    {
        //        context.Result = new RedirectResult((new UrlHelper(context)).Action("Logout", "Account"
        //            , new { companycode = context.HttpContext.Request.Query["companycode"] }));
        //    }
        //    base.OnActionExecuting(context);
        //}

        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
           
        //}
    }
}
