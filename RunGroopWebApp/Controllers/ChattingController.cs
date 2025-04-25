using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers
{
    public class ChattingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Notification()
        {
            return View();
        }


    }
}
