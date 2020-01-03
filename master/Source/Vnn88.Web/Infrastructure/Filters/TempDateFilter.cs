using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Vnn88.Common.Infrastructure;

namespace Vnn88.Web.Infrastructure.Filters
{
    public class TempDateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var validKey = new[]
            {
                nameof(NotifyMessage)
            };

            var controllerName = (context.RouteData.Values[Constants.Controller] + Constants.Controller).ToLower();
            var tempData = (context.Controller as Controller)?.TempData;
            if (tempData != null)
                foreach (var key in tempData.Keys.ToList())
                {
                    if (!key.Equals(controllerName, StringComparison.OrdinalIgnoreCase) && !validKey.Contains(key))
                    {
                        tempData.Remove(key);
                    }
                }
            base.OnActionExecuting(context);
        }
    }
}
