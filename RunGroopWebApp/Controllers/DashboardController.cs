
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Interfaces.Services;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    [Authorize]
    public class DashboardController(IDashboardRepository _dashboardRespository, IPhotoService _photoService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRespository.GetAllUserRaces();
            var userClubs = await _dashboardRespository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }
    }
}