using GraphQL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RunGroop.Application.Helpers;
using RunGroop.Application.ViewModels;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Interfaces;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController(IUnitOfWork _UnitOfWork ) : Controller
    {

        [Route("RunningClubs")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int category = 0)
        {
            var clubs = await _UnitOfWork.ClubRepository.GetAll();

            //if (clubs == null || !clubs.Any())
            //    return NotFound();

            var filteredClubs = clubs.Where(c => c.ClubCategory == (ClubCategory)category || category == 0);

            var totalClubs = filteredClubs.Count();
            var totalPages = (int)Math.Ceiling(totalClubs / (double)pageSize);
            var clubsForPage = filteredClubs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new IndexClubViewModel
            {
                Clubs = clubsForPage,
                PageSize = pageSize,
                Page = page,
                TotalPages = totalPages,
                TotalClubs = totalClubs,
                Category = category
            };

            return View(viewModel); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var club= await _UnitOfWork.ClubRepository.GetByIdAsync(id);
            if (club == null) return NotFound();
            return View(club);
        }

        [HttpGet]
        [Route("RunningClubs/{state}")]
        public async Task<IActionResult> ListClubsByState(string state)
        {
            var clubs = await _UnitOfWork.ClubRepository.GetClubsByState(StateConverter.GetStateByName(state).ToString());
            var clubVM = new ListClubByStateViewModel()
            {
                Clubs = clubs
            };
            if (!clubs.Any())
            {
                clubVM.NoClubWarning = true;
            }
            else
            {
                clubVM.State = state;
            }
            return View(clubVM);
        }

        [Route("RunningClubs/{city}/{state}")]
        public async Task<IActionResult> ListClubsByCity(ListClubByCityViewModel cityList)
        {
            var clubs = await _UnitOfWork.ClubRepository.GetClubByCity(cityList.City);
            var clubVM = new ListClubByCityToReturnViewModel()
            {
                Clubs = clubs
            };
            if (!clubs.Any())
            {
                clubVM.NoClubWarning = true;
            }
            else
            {
                clubVM.State = cityList. State;
                clubVM.City = cityList .City;
            }
            return View(clubVM);
        }

        [Route("club/{runningClub}/{id}")]
        public async Task<IActionResult> DetailClub(ClubDetailsViewModel cl)
        {
            var club = await _UnitOfWork.ClubRepository.GetByIdAsync(cl.Id);

            return club == null ? NotFound() : View(club);
        }

        [HttpGet]
        [Route("RunningClubs/State")]
        public async Task<IActionResult> RunningClubsByStateDirectory()
        {
            var states = await _UnitOfWork.ClubRepository.GetAllStates();
            var clubVM = new RunningClubByState()
            {
                States = states
            };

            return states == null ? NotFound() : View(clubVM);
        }

        [HttpGet]
        [Route("RunningClubs/State/City")]
        public async Task<IActionResult> RunningClubsByStateForCityDirectory()
        {
            var states = await _UnitOfWork.ClubRepository.GetAllStates();
            var clubVM = new RunningClubByState()
            {
                States = states
            };

            return states == null ? NotFound() : View(clubVM);
        }

        [HttpGet]
        [Route("RunningClubs/{state}/City")]
        public async Task<IActionResult> RunningClubsByCityDirectory(string state)//primitive data are binded automatically 
        {
            var cities = await _UnitOfWork.ClubRepository.GetAllCitiesByState(StateConverter.GetStateByName(state).ToString());
            var clubVM = new RunningClubByCity()
            {
                Cities = cities
            };

            return cities == null ? NotFound() : View(clubVM);
        }
        public IActionResult Create()
        {
            var curUserId = HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel { AppUserId = curUserId };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = clubVM.Image.ToString(),
                    ClubCategory = clubVM.ClubCategory,
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    }
                };
                _UnitOfWork.ClubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _UnitOfWork.ClubRepository.GetByIdAsync(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _UnitOfWork.ClubRepository.GetByIdAsyncNoTracking(id);

            if (userClub == null)
            {
                return View("Error");
            }

            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = clubVM.Image.ToString(),
                AddressId = clubVM.AddressId,
                Address = clubVM.Address,
            };

            _UnitOfWork.ClubRepository.Update(club);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _UnitOfWork.ClubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _UnitOfWork.ClubRepository.GetByIdAsync(id);

            if (clubDetails == null)
            {
                return View("Error");
            }

            _UnitOfWork.ClubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }
     
    }
}