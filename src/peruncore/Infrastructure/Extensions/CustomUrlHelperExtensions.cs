using Microsoft.AspNetCore.Mvc;

namespace peruncore.Infrastructure.Extensions
{
    public static class CustomUrlHelperExtensions
    {
        public static string AbsoluteAction(
            this IUrlHelper url,
            string actionName,
            string controllerName,
            object routeValues = null)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }
    }
}
