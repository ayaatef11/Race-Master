using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using RunGroop.Data.Data.Enum;

namespace RunGroop.Application.Filters
{
    /*The CustomHeaderAttribute is a filter that appends a custom HTTP header to responses.
     * It is useful for adding metadata or security headers to responses consistently throughout the application
     * , enhancing maintainability and separation of concerns. Filters like this are valuable in scenarios 
     * where you need to inject custom behavior into
     * the response pipeline, without cluttering individual controller actions with repetitive code.*/
    public class CutsomHeaderAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Append(CustomHeaderNames.CustomAddName, "custom header value");
            base.OnResultExecuting(context);
        }
    }
}
