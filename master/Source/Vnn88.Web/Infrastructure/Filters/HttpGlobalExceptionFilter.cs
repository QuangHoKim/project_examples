using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;
using Vnn88.Common.Infrastructure;
using Vnn88.Web.Infrastructure.ActionResults;
using Vnn88.Web.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
namespace Vnn88.Web.Infrastructure.Filters
{
    /// <inheritdoc />
    /// <summary>
    /// Http Global Exception Filter Class
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor for Class
        /// </summary>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public HttpGlobalExceptionFilter(IHostingEnvironment env,
            ILogger<HttpGlobalExceptionFilter> logger, IConfiguration configuration)
        {
            _env = env;
            _logger = logger;
            _configuration = configuration;
        }

        /// <inheritdoc />
        /// <summary>
        /// On Exception Class
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            if (context.Exception.GetType() == typeof(ApiDomainException))
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] {context.Exception.Message}
                };

                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] {_configuration[Constants.Settings.ErrorMessage] }
                };

                if (_env.IsDevelopment())
                {
                    json.DeveloperMeesage = context.Exception;
                }

                context.Result = new InternalServerErrorObjectResult(json);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Json Error Response Class
        /// </summary>
        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public object DeveloperMeesage { get; set; }
        }
    }
}