using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace RunGroop.Application.MiddleWares
{
    public class RequestCultureMiddleWare(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var currentLanguage = context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            var browserLanguage = context.Request.Headers["Accept-Language"].ToString()[..2];//extracts the first two characters 

            if (string.IsNullOrEmpty(currentLanguage))
            {
                var culture = browserLanguage switch
                {
                    "ar" => "ar-EG",
                    "de" => "de-DE",
                    _ => "en-US",
                };
                var requestCulture = new RequestCulture(culture, culture);

                context.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, null)); 
                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
            }

            await _next(context);
        }
    }
}
