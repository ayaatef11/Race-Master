
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Data;
using RunGroop.Data.Models.Identity;
using RunGroopWebApp.Data;
using RunGroopWebApp.Helpers;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;


namespace RunGroopWebApp.Controllers
{
    public class HomeController(ILogger<HomeController> _logger, IClubRepository _clubRepository,
            UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, ILocationService _locationService, IConfiguration _config, IStringLocalizer<HomeController> _localizer) : Controller
    {

		//tempData : is designed for short-lived data. The data persists only until it is read once. After the data is read, it is automatically removed,
		//making it perfect for scenarios where data needs to be carried forward only for a single subsequent request.
		//is stored in session state, so it shares some of the characteristics of session management, including dependency on session storage settings.
//Commonly used for passing error messages, success notifications, or any other data that needs to be transferred between requests, especially during a redirect.
//It stores data in a dictionary-like structure(IDictionary<string, object>) and can hold any type of data.
		public async Task<IActionResult> Index()
		{
			ViewBag.WelcomeMessage = string.Format(_localizer["welcome"], "DevCreed");//support multiple languages and cultures
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
                    homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
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

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel homeVM)
        {
            var createVM = homeVM.Register;

            if (!ModelState.IsValid) return View(homeVM);

            var user = await _userManager.FindByEmailAsync(createVM.Email);
            if (user != null)
            {
                ModelState.AddModelError("Register.Email", "This email address is already in use");
                return View(homeVM);
            }

            var userLocation = await _locationService.GetCityByZipCode(createVM.ZipCode ?? 0);

            if (userLocation == null)
            {
                ModelState.AddModelError("Register.ZipCode", "Could not find zip code!");
                return View(homeVM);
            }

            var newUser = new AppUser
            {
                UserName = createVM.UserName,
                Email = createVM.Email,
                Address = new Address()
                {
                    State = userLocation.StateCode,
                    City = userLocation.CityName,
                    ZipCode = createVM.ZipCode ?? 0,
                }
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, createVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Index", "Club");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        //to give me the culture and the return url to reload you in the same page when you change the language and not turn you to another page 
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,//This method is used to add or update a cookie in the HTTP response.
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