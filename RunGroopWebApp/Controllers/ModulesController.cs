using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers
{
    public class ModulesController : Controller
    {
        public IActionResult Module1()
        {
            return View();
        }
        public IActionResult Module2()
        {
            return View();
        }
        public IActionResult Module3()
        {
            return View();
        }
    }
}
