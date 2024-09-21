using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtractHeaderController : ControllerBase
    {
        public IActionResult Extract()
        {

            Request.Headers.TryGetValue(CustomHeaderNames.CustomExtractName, out var headerValue);

            return Ok(headerValue);
        }
        [HttpGet("fromheader")]
        public IActionResult ExtractFromQueryAttribute([FromHeader] HeaderDTO headerDTO)
        {

            return Ok(headerDTO);
        }
        [HttpGet("actionfilter")]
        [ExtractCustomHeader]
        public IActionResult ExtractFromFilter()
        {

            HttpContext.Items.TryGetValue(CustomHeaderNames.CustomExtractName, out var headerValue);

            return Ok(headerValue);
        }
        [HttpGet("middleware")]
        public IActionResult ExtractFromMiddleware()
        {

            HttpContext.Items.TryGetValue(CustomHeaderNames.CustomExtractName, out object? headerValue);

            return Ok(headerValue);
        }
    }
}