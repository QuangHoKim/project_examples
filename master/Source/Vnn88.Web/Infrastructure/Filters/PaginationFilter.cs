using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Vnn88.Common.Infrastructure;

namespace Vnn88.Web.Infrastructure.Filters
{
    public class PaginationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var allowActions = new[]
            {
                "Get",
                "GetAll"
            }.Select(m => m.ToLower());

            var controllerName = (context.RouteData.Values[Constants.Controller] + Constants.Controller).ToLower();
            var actionName = context.RouteData.Values[Constants.Action].ToString().ToLower();

            var tempData = (context.Controller as Controller)?.TempData;

            //if (allowActions.Contains(actionName))
            //{
            //    var formRequest = context.HttpContext.Request.Form;
            //    DataTableParam dataTableParam = new DataTableParam
            //    {
            //        Start = !string.IsNullOrEmpty(formRequest[Constants.Pagination.Start]) ? int.Parse(formRequest[Constants.Pagination.Start]) : 0,
            //        SearchValue = !string.IsNullOrEmpty(formRequest[Constants.Pagination.SearchValue])
            //            ? formRequest[Constants.Pagination.SearchValue].ToString()
            //            : string.Empty,
            //        OrderColumn = !string.IsNullOrEmpty(formRequest[Constants.Pagination.OrderColumn])
            //            ? int.Parse(formRequest[Constants.Pagination.OrderColumn])
            //            : 1,
            //        OrderDir = !string.IsNullOrEmpty(formRequest[Constants.Pagination.OrderDirection])
            //            ? formRequest[Constants.Pagination.OrderDirection].ToString()
            //            : Constants.Pagination.DefaultOrder
            //    };

            //    tempData?.Put(controllerName, dataTableParam);
            //}

            base.OnActionExecuting(context);
        }
    }
}
