
using RunGroopWebApp.ViewModels;
using RunGroop.Data.Models.Identity;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using RunGroop.Repository.Interfaces;
using System.Text.Json;
using System.Net.Http.Headers;
/*Key Concepts of Sessions in ASP.NET Core MVC

1. Session State:

· Session state allows you to store user-specific data on the server side, and it's associated
with a unique session ID sent to the client via a cookie or other mechanisms.

. It's commonly used to keep track of user activities, preferences, and other temporary data
throughout the user's visit to the website.

2. Session Storage:

. Data stored in a session is generally held in server memory. However, it can also be
configured to use other storage providers like distributed caches or databases for
scalability.

3. Session ID:

. The session ID is a unique identifier for each user's session, which is sent to the client in a
cookie. The server uses this ID to retripe session data for subsequent requests.*/
/*private readonly Lazy<IOwnerService> _lazyOwnerService;

public ServiceManager(IRepositoryManager repositoryManager)

_lazyOwnerService = new Lazy<IOwnerService>(()=> new OwnerService(repositoryManager));

}

public IOwnerService OwnerService => _lazyOwnerService.Value;*/
namespace RunGroopWebApp.Controllers
{
    public class UserController(IUnitOfWork _UnitOfWork, UserManager<AppUser> _userManager, IPhotoService _photoService,HttpClient _httpClient) : Controller
    {
        private readonly JsonSerializerOptions _serializerOptions=new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; 
         [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _UnitOfWork.UserRepository.GetAll();
			List<UserViewModel> result = new();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    Pace = user.Pace,
                    City = user.City,
                    State = user.State,
                    Mileage = user.Mileage,
                    UserName = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-4.jpg",
                };
                result.Add(userViewModel);
            }
			var userInfo = JsonConvert.DeserializeObject<UserViewModel>(HttpContext.Session.GetString("SessionUser"));

			return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _UnitOfWork.UserRepository.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");//view all users 
            }

            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                Pace = user.Pace,
                City = user.City,
                State = user.State,
                Mileage = user.Mileage,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-4.jpg",
            };
            return View(userDetailViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            var editMV = new EditProfileViewModel()
            {
                City = user.City,
                State = user.State,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return View(editMV);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditProfile", editVM);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }

            if (editVM.Image != null) 
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                    return View("EditProfile", editVM);
                }

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                user.ProfileImageUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = user.ProfileImageUrl;

                await _userManager.UpdateAsync(user);

                return View(editVM);
            }

            user.City = editVM.City;
            user.State = editVM.State;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Detail", "User", new { user.Id });
        }
        public async Task<IEnumerable<UserModel> GetUsersAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await _httpClient.GetAsync("users");
            result.EnsureSuccessStatusCode();
            var response=await result.Content.ReadAsStringAsync();
            return JsonSer ializer.Deserialize<IEnumerable<UserModel>>(response, _serializerOptions);
        
        }
    }
}