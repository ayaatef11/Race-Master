using MediatR;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Interfaces;
using RunGroopWebApp.Commands;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Extensions;
using RunGroopWebApp.Helpers;
using RunGroopWebApp.Queries;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController(ISender _sender,IMediator mediator, IPublisher _publisher, IUnitOfWork _UnitOfWork, IPhotoService _photoService) : Controller
    {

        [Route("RunningClubs")]
        public async Task<IActionResult> Index()
        {
            var query = new GetAllClubsQuery();
            var response = mediator.Send(query);

            return View(response);
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
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
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

            var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(clubVM);
            }

            if (!string.IsNullOrEmpty(userClub.Image))
            {
                _ = _photoService.DeletePhotoAsync(userClub.Image);
            }

            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = photoResult.Url.ToString(),
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

            if (!string.IsNullOrEmpty(clubDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(clubDetails.Image);
            }

            _UnitOfWork.ClubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }

        /*private void btnLoadIntoMap_Click(object sender,

map.DragButton = MouseButtons. Righth
map.MapProvider = GMapProviders.GoogleMap;
double lat = Convert.ToDouble(txtLat.Text);
double longt = Convert.ToDouble(txtLong.Text)
map.Position = new PointLatLng(lat, longt);
map.MinZoom = 5; // Minimum Zoom Level
map.MaxZoom = 100; // Maximum Zoom Level
map.Zoom = 10; // Current Zoom Level*/
        public async Task< IActionResult> CreatingClub(CreateClubViewModel newBooking)//we can update it to publish notifications
        {

            if (!ModelState.IsValid) return BadRequest();
            _sender.Send(new CreateClubRequest(newBooking));
            await _publisher.Publish(new CreateClubRequest(newBooking));
           return Ok();//it is better to return the product to see if it is returned actually or not 
        }
    }
}