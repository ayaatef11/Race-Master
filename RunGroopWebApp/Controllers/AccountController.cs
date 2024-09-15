
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Identity;
using RunGroopWebApp.Data;
using RunGroopWebApp.ViewModels;
using RunGroop.Data.Interfaces.Services;
//c#logging
/*Trace = 0

Debug = 1

Information = 2

Warning =3

Updates
eRight

Error = 4

Critical = 5

rts

None = 6

Log level for very low severity diagnostic messages.

Log level for low severity diagnostic messages.

Log level for informational diagnostic messages.

Log level for diagnostic messages that indicate a non-
fatal problem.

Log level for diagnostic messages that indicate a failure
in the current operation.

Log level for diagnostic messages that indicate a failure
that will terminate the entire application.

The highest possible log level. Used when configuring
logging to indicate that no log messages should be
emitted.*/
/*UserManager & SignInManager

x

UserManager<IdentityUser>
CreateAsync
DeleteAsync
UpdateAsync
Etc ...

SignInManager<IdentityUser>
SignlnAsync
SignOutAsync
IsSignedln*/

/*[HttpPost]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto 
userForRegistration)
{
var result = await
_service.AuthenticationService.RegisterUser(userForRegistration);
if (!result.Succeeded)
 
285
{
foreach (var error in result.Errors)
{
ModelState.TryAddModelError(error.Code, error.Description);
}
return BadRequest(ModelState);
}
return StatusCode(201);
}
*/
namespace RunGroopWebApp.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,
            SignInManager<AppUser> _signInManager,
            ApplicationDbContext _context,
            ILocationService _locationService) : Controller
    {


        public IActionResult Login()
        {
            var response = new LoginViewModel();
            if (!ModelState.IsValid)
            {
                // Model validation failed, show errors to the user
                return View(response);
            }

            // Proceed with login logic
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

            // Generate the token for two-factor authentication
            var token = await _userManager.GenerateUserTokenAsync(user, "Default", purpose);
            return token;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        {

            // Generate the token for email confirmation
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;

        }

    }
}