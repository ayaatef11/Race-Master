﻿
using RunGroopWebApp.ViewModels;
using RunGroop.Data.Models.Identity;
using RunGroop.Repository.Interfaces;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GraphQL;
using RunGroop.Application.ViewModels;
using x=Newtonsoft.Json.JsonConvert;
namespace RunGroopWebApp.Controllers
{
    public class UserController(IUnitOfWork _UnitOfWork, UserManager<AppUser> _userManager, HttpClient _httpClient) : Controller
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
                return RedirectToAction("Index", "Users");
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
                user = new AppUser();
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
        public async Task<IEnumerable<UserModel>> GetUsersAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await _httpClient.GetAsync("users");
            result.EnsureSuccessStatusCode();
            var response=await result.Content.ReadAsStringAsync();
            return x.DeserializeObject<IEnumerable<UserModel>>(response);
        }
    }
}