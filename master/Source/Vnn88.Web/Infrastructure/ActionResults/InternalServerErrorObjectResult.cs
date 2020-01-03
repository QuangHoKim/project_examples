using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vnn88.Web.Infrastructure.ActionResults
{
    /// <summary>
    /// Internal Server Error Object Result Class
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Constructor for Class
        /// </summary>
        /// <param name="error"></param>
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
