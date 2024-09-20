using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers
{
    public class ConnectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
