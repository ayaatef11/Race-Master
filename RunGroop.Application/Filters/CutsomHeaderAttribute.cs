using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using RunGroop.Data.Data.Enum;

namespace RunGroop.Application.Filters
{
    public class CutsomHeaderAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Append(CustomHeaderNames.CustomAddName, "custom header value");
            base.OnResultExecuting(context);
        }
    }
}
