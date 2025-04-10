
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Identity;
using RunGroopWebApp.ViewModels;
using RunGroop.Data.Data.Enum;
using RunGroop.Data.Data;
using RunGroop.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


namespace RunGroopWebApp.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,
            SignInManager<AppUser> _signInManager,
            ILocationService _locationService, IUnitOfWork object1/*,IdentityServer4.Services.ITokenService object2*/) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            //if (!ModelState.IsValid)
            //{
                return View(response);
            //}
            //return RedirectToAction("Index", "Home");
        }
     

	[HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var response = new HomeUserCreateViewModel();
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> Register(HomeUserCreateViewModel createVM)
        {

            if (!ModelState.IsValid) return View(createVM);

            var user = await _userManager.FindByEmailAsync(createVM.Email);
            if (user != null)
            {
                ModelState.AddModelError("Register.Email", "This email address is already in use");
                return View(createVM);
            }

            var userLocation = await _locationService.GetCityByZipCode(createVM.ZipCode ?? 0);

            //if (userLocation == null)
            //{
            //    ModelState.AddModelError("Register.ZipCode", "Could not find zip code!");
            //    return View(homeVM);
            //}

            var newUser = new AppUser
            {
                UserName = createVM.UserName,
                Email = createVM.Email
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, createVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Index", "Club");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Race");
        }

        public async Task<IActionResult> GetLocation(string location)
        {
            if(location == null)
            {
                return Json("Not found");
            }
            var locationResult = await _locationService.GetLocationSearch(location);
            return Json(locationResult);
        }
        public async Task<string> GenerateTwoFactorTokenAsync(AppUser user, string purpose)
        {
            var token = await _userManager.GenerateUserTokenAsync(user, "Default", purpose);
            return token;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;

        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(WelcomeViewModel model)
        {
            if (model.Image == null || model.Image.Length == 0)
            {
                ModelState.AddModelError("Image", "Please select a valid image.");
                return View(model);
            }
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Image.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(fileStream);
            }
            TempData["Success"] = "Image uploaded successfully!";
            return RedirectToAction("Welcome"); 
        }
        [HttpGet]
        public IActionResult Welcome()
        {
            var model = new WelcomeViewModel();
            return View(model);
        }

    }

}
