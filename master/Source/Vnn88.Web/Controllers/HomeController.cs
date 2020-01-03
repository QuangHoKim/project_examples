using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vnn88.Web.Infrastructure.Filters;

namespace Vnn88.Web.Controllers
{
    /// <inheritdoc />
    [Authorize]
    [ServiceFilter(typeof(AuthorizeLoginFilter))]
    public class HomeController : Controller
    {
        /// <summary>
        /// Dashboard page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}