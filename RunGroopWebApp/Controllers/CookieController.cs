using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers
{
    public class CookieController : Controller
    {
        public IActionResult SetCookie()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true, 
                SameSite = SameSiteMode.Strict, 
                Expires = DateTime.Now.AddDays(1) 
            };
            Response.Cookies.Append("auth-token", "your-token-value", cookieOptions);
            return Ok("HttpOnly cookie has been set.");
        }

        public IActionResult GetCookie()
        {
            var authToken = Request.Cookies["auth-token"];

            if (authToken != null)
            {
                return Ok($"Your auth token is: {authToken}");
            }
            else
            {
                return Ok("No auth token found.");
            }
        }
    }
}
