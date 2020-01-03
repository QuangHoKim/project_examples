using Microsoft.AspNetCore.Builder;

namespace Vnn88.Web.Infrastructure.Middlewares
{
    /// <summary>
    /// Authentication Pipeline Class
    /// </summary>
    public class AuthenticationPipeline
    {
        /// <summary>
        /// Configure Function
        /// </summary>
        /// <param name="applicationBuilder"></param>
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
