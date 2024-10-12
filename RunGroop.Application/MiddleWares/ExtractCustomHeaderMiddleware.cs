using Microsoft.AspNetCore.Http;
using RunGroop.Data.Data.Enum;

namespace RunGroop.Application.MiddleWares
{
    public class ExtractCustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public ExtractCustomHeaderMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {

            context.Request.Headers.TryGetValue(CustomHeaderNames.CustomExtractName, out var headerValue);

            if (context.Items.ContainsKey(CustomHeaderNames.CustomExtractName))
                context.Items[CustomHeaderNames.CustomExtractName] = headerValue;
            else
                context.Items.Add(CustomHeaderNames.CustomExtractName, headerValue);

            await _next(context);
        }
    }
}