using Microsoft.AspNetCore.Builder;
using RunGroopWebApp.MiddleWares;

namespace RunGroopWebApp.Extensions
{
    public static class RequestCultureMiddleWareExtension
    {
       public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<RequestCultureMiddleWare>();
            }
        
    }
}

