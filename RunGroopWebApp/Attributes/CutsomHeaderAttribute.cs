using Microsoft.AspNetCore.Mvc.Filters;

namespace RunGroopWebApp.Attributes
{
    public class CutsomHeaderAttribute:ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Append(CustomHeaderNames.CustomAddName, "custom header value")
            base.OnResultExecuting(context);
        }
    }
}
