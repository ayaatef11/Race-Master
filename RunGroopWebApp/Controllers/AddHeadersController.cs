using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddHeadersController : ControllerBase
    {
        [CustomHeader]
        public IActionResult CustomHeaderResponse()
        {

            //HttpContext.Response.Headers.Append( CustomHeaderNames.CustomAddName, "custom header value");

            return Ok();
        }
    }
}
