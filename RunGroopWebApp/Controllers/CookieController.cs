using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace RunGroopWebApp.Controllers
{
    public class CookieController : Controller
    {
        public IActionResult SetCookie()
        {
            // Define the cookie options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // This makes the cookie inaccessible to JavaScript
                Secure = true, // Ensures the cookie is only sent over HTTPS
                SameSite = SameSiteMode.Strict, // Restricts cross-site cookie usage
                Expires = DateTime.Now.AddDays(1) // Sets the cookie to expire in 1 day
            };

            // Add the cookie to the response
            Response.Cookies.Append("auth-token", "your-token-value", cookieOptions);

            return Ok("HttpOnly cookie has been set.");
        }

        public IActionResult GetCookie()
        {
            // Retrieve the cookie from the request
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
