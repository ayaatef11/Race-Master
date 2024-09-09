using Microsoft.AspNetCore.Localization;

namespace RunGroopWebApp.MiddleWares
{
    public class RequestCultureMiddleWare(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var currentLanguage = context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            var browserLanguage = context.Request.Headers["Accept-Language"].ToString()[..2];//extracts the first two characters 

            if (string.IsNullOrEmpty(currentLanguage))
            {
                var culture  = browserLanguage switch
				{
					"ar" => "ar-EG",
					"de" => "de-DE",
					_ => "en-US",
				};
				var requestCulture = new RequestCulture(culture, culture);
				/*Features is a property of HttpContext that allows you to get or set additional information related to the request through various feature interfaces.
It works like a collection where you can add, modify, or retrieve specific features related to the request.*/
				context.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture, null));
				/*CurrentCulture: Impacts data presentation and formatting based on the user’s culture.
CurrentUICulture: Impacts the language of the user interface.*/
				CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
            }

            await _next(context);
        }
    }
}
