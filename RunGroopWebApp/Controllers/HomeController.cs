
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Models.Data;
using RunGroop.Data.Models.Identity;
//using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using RunGroop.Repository.Interfaces;
using RunGroop.Application.Helpers;
using RunGroop.Data.Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Newtonsoft.Json;
using System.Diagnostics;
using RunGroopWebApp.Services.Services.interfaces;


namespace RunGroopWebApp.Controllers
{
    public class HomeController(ILogger<HomeController> _logger, IUnitOfWork _UnitOfWork,
            UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, ILocationService _locationService, IConfiguration _config, IStringLocalizer<HomeController> _localizer) : Controller
    {
		public async Task<IActionResult> Index()
		{
			ViewBag.WelcomeMessage = string.Format(_localizer["welcome"], "Customer");//support multiple languages and cultures
			var ipInfo = new IPInfo();//ip information like city,country and region
            var homeViewModel = new HomeViewModel();
            try
            {
                string url = "https://ipinfo.io?token=" + _config.GetValue<string>("IPInfoToken");//allow secure access to ip info service
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpResponseMessage response = await client.GetAsync(url);//crosss error 
                    response.EnsureSuccessStatusCode();

                    string info = await response.Content.ReadAsStringAsync();
                    ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                    RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                    ipInfo.Country = myRI1.EnglishName;
                    homeViewModel.City = ipInfo.City;
                    homeViewModel.State = ipInfo.Region;
                }
                if (homeViewModel.City != null)
                {
                    homeViewModel.Clubs = await _UnitOfWork.ClubRepository.GetClubByCity(homeViewModel.City);
                }
                return View(homeViewModel);
            }
            catch (Exception)
            {
                homeViewModel.Clubs = null;
            }

            return View(homeViewModel);
        }

        public IActionResult Register() 
        {
            var response = new HomeUserCreateViewModel();
            return View(response);
        }

        //[HttpPost]
        //public async Task<IActionResult> Index(HomeViewModel homeVM)
        //{
        //    var createVM = homeVM.Register;

        //    if (!ModelState.IsValid) return View(homeVM);

        //    var user = await _userManager.FindByEmailAsync(createVM.Email);
        //    if (user != null)
        //    {
        //        ModelState.AddModelError("Register.Email", "This email address is already in use");
        //        return View(homeVM);
        //    }

        //    var userLocation = await _locationService.GetCityByZipCode(createVM.ZipCode ?? 0);

        //    //if (userLocation == null)
        //    //{
        //    //    ModelState.AddModelError("Register.ZipCode", "Could not find zip code!");
        //    //    return View(homeVM);
        //    //}

        //    var newUser = new AppUser
        //    {
        //        UserName = createVM.UserName,
        //        Email = createVM.Email
        //    };

        //    var newUserResponse = await _userManager.CreateAsync(newUser, createVM.Password);

        //    if (newUserResponse.Succeeded)
        //    {
        //        await _signInManager.SignInAsync(newUser, isPersistent: false);
        //        await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        //    }
        //    return RedirectToAction("Index", "Club");
        //}

        public IActionResult Privacy()
        {
            ViewBag.WelcomeMessage = string.Format(_localizer["welcome"], "aya atef");//here you pass the arguemnt 0 with aya atef
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),//This method generates the value to be stored in the cookie. It takes a RequestCulture object, which contains the culture information.

                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}