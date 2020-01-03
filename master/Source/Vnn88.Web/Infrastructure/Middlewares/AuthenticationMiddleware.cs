using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using Vnn88.Common.Infrastructure;

namespace Vnn88.Web.Infrastructure.Middlewares
{
    /// <summary>
    /// Authentication Middleware Class
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor for Class
        /// </summary>
        /// <param name="next"></param>
        /// <param name="configuration"></param>
        public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        /// <summary>
        /// Invoke Function
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers[Constants.BasicAuth.Authorization];
            if (authHeader != null && authHeader.StartsWith(Constants.BasicAuth.Basic))
            {
                // Extract credentials
                var encodedUsernamePassword = authHeader.Substring(Constants.BasicAuth.BasicHeader.Length)
                    .Trim();
                var encoding = Encoding.GetEncoding(Constants.BasicAuth.Iso88591);
                string usernamePassword;
                try
                {
                    usernamePassword =
                        encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                }
                catch (Exception)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }
                var seperatorIndex = usernamePassword.IndexOf(Constants.Colon);
                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                if (username == _configuration[Constants.BasicAuth.AuthenticationUsername] 
                    && password == _configuration[Constants.BasicAuth.AuthenticationPassword])
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401; // Unauthorized
                }
            }
            else
            {
                // No authorization header
                context.Response.StatusCode = 401; // Unauthorized
            }
        }
    }
}
