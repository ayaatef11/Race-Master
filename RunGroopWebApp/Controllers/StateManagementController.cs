using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Channels;

namespace RunGroopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateManagementController : ControllerBase
    {
        public IActionResult SetSession(string name,int age) {

            HttpContext.Session.SetString("Name", name);
            HttpContext.Session.SetInt32("Age", age);

            return Content("session data Saved");
        }

        public IActionResult GetSessionData()
        {

            string name = HttpContext.Session.GetString("Name");

            int? age = HttpContext.Session.GetInt32("Age");
            return Content($"data Name={name}Age ={age}");
        }

    }
}
