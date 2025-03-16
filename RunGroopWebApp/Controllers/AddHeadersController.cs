using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunGroop.Data.Data.Enum;

namespace RunGroopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddHeadersController : ControllerBase
    {
       //[CustomHeader]
        public IActionResult CustomHeaderResponse()
        {

            HttpContext.Response.Headers.Append( CustomHeaderNames.CustomAddName, "custom header value");

            return Ok();
        }
    }
}
