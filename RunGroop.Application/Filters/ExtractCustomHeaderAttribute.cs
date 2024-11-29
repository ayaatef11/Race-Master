using Microsoft.AspNetCore.Mvc.Filters;
using RunGroop.Data.Data.Enum;

namespace RunGroop.Application.Filters
{
    public class ExtractCustomHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            context.HttpContext.Request.Headers.TryGetValue(CustomHeaderNames.CustomExtractName, out var headerValue);

            if (context.HttpContext.Items.ContainsKey(CustomHeaderNames.CustomExtractName))
                context.HttpContext.Items[CustomHeaderNames.CustomExtractName] = headerValue;
            else
                context.HttpContext.Items.Add(CustomHeaderNames.CustomExtractName, headerValue);
        }
    }
}
