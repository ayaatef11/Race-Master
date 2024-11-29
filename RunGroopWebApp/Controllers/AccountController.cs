
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
            ApplicationDbContext _context,
            ILocationService _locationService, IUnitOfWork object1/*,IdentityServer4.Services.ITokenService object2*/) : Controller
    {
        public IUnitOfWork Object1 { get; } = object1;

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            if (!ModelState.IsValid)
            {
                return View(response);
            }
            return RedirectToAction("Index", "Home");
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

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Index", "Race");
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

    }
}