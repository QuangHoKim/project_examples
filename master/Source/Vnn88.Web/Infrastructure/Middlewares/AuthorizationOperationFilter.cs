using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Vnn88.Common.Infrastructure;

namespace Vnn88.Web.Infrastructure.Middlewares
{
    /// <summary>
    /// Authorization Operation Filter Class
    /// </summary>
    public class AuthorizationOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply Authorization Operation Filter
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = Constants.BasicAuth.Authorization,
                In = Constants.BasicAuth.Header,
                Description = Constants.BasicAuth.BasicAuthorization,
                Required = true,
                Type = Constants.BasicAuth.StringType
            });
        }
    }
}
