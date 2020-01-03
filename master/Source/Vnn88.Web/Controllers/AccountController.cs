using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vnn88.Common.Resources;
using Microsoft.Extensions.Configuration;
using Vnn88.DataModel;
using Vnn88.Common.Infrastructure;
using Vnn88.Service;
using Vnn88.Web.Infrastructure.Filters;

namespace Vnn88.Web.Controllers
{
    /// <inheritdoc />
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersService _usersService;
        private readonly HttpContext _httpContext;

        /// <summary>
        /// AccountController Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="usersService"></param>
        public AccountController(IConfiguration configuration,
            IHttpContextAccessor contextAccessor, IUsersService usersService)
        {
            _configuration = configuration;
            _usersService = usersService;
            _httpContext = contextAccessor.HttpContext;
        }

        /// <summary>
        /// Redirect to login view
        /// </summary>
        /// <returns>Login View</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginModel { ReturnUrl = returnUrl };
            return View(model);
        }
        /// <summary>
        /// Check Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _usersService.Login(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, MessageResource.UserLoginFailed);
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(Constants.ClaimName.UserName, user.UserName),
                    new Claim(Constants.ClaimName.AccountId, user.Id.ToString()),
                    new Claim(Constants.ClaimName.Role, user.Role.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                // Storage cookies
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                    {
                        IsPersistent = model.Remember,
                        ExpiresUtc =
                            DateTime.UtcNow.AddMinutes(
                                _configuration.GetValue<int>(Constants.Settings
                                    .LoginExpiredTime))
                    });
                Response.Cookies.Append(Constants.ClaimName.UserName, model.UserName, new CookieOptions { Expires = DateTime.UtcNow.AddDays(30) });
                return RedirectToLocal(model.ReturnUrl);
            }
            return View(model);
        }
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Save system log
            HttpContext.Session.Remove(Constants.SessionKey);
            Response.Cookies.Delete(Constants.ClaimName.UserName);
            return RedirectToAction("Login");
        }
        /// <summary>
        /// Redirect to Home Page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [ServiceFilter(typeof(AuthorizeLoginFilter))]
        [HttpGet]
        public IActionResult ChangePass()
        {
            var changePassModel = new ChangePasswordModel
            {
                Id = _httpContext.User.GetUserId()
            };
            return View(changePassModel);
        }
        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangePass(ChangePasswordModel model)
        {
            try
            {
                if (_usersService.ChangePass(model))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, MessageResource.ChangePassFailed);
                return View(model);
            }
            return View(model);
        }
        
    }
}