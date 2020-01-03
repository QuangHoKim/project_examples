using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Vnn88.Common.Infrastructure
{
    public static class HtmlExtensions
    {

        public static IHtmlContent HelloWorldHtmlString(this IHtmlHelper htmlHelper)
            => new HtmlString("<strong>Hello World</strong>");

        public static String HelloWorldString(this IHtmlHelper htmlHelper)
            => "<strong>Hello World</strong>";
    }
}
