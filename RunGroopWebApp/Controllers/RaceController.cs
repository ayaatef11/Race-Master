
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RunGroop.Application.ViewModels;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Interfaces;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Extensions;
using RunGroopWebApp.Services.interfaces;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController(SemaphoreSlim semaphore,ILogger<RaceController>logger,IMemoryCache memoryCache ,IUnitOfWork _UnitOfWork,  IHttpContextAccessor _httpContextAccessor,INotificationService _notificationService) : Controller
    {
        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 6)
        {
            await _notificationService.SendNotificationToUserAsync("user-id", "Welcome to the app!");

            //if (page < 1 || pageSize < 1)
            //{
            //    return NotFound();
            //}

            // if category is -1 (All) dont filter else filter by selected category
            var races = category switch
            {
                -1 => await _UnitOfWork.RaceRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _UnitOfWork.RaceRepository.GetRacesByCategoryAndSliceAsync((RaceCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _UnitOfWork.RaceRepository.GetCountAsync(),
                _ => await _UnitOfWork.RaceRepository.GetCountByCategoryAsync((RaceCategory)category),
            };

            var viewModel = new IndexRaceViewModel
            {
                Races = races,
                Page = page,
                PageSize = pageSize,
                TotalRaces = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };

            return View(viewModel);
        }

        [Route("event/{runningRace}/{id}")]
        public async Task<IActionResult> DetailRace(RaceDetailViewModel dd)
        {
            //get it from the cache 
            if (memoryCache.TryGetValue(dd.Id, out Race race)) logger.LogInformation("Employees found in cache");
            else
            {
                //if it isn't found in the cache get it from the database and then cache it with the duration specified 
                logger.LogInformation("Employees not found in cache. Fetching  from the database");
                race = await _UnitOfWork.RaceRepository.GetByIdAsync(dd.Id);

                var cacheEntryOptions=new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                    .SetPriority(CacheItemPriority.Normal).SetSize(1);

               memoryCache.Set(dd.Id, race,cacheEntryOptions);
            }
            semaphore.Release();
            return race == null ? NotFound() : View(race);
        }

        private string GetInstanceId()
        {

            var instanceId = HttpContext.Session.GetString("InstanceId");
            if (string.IsNullOrEmpty(instanceId))
            {
                instanceId = Guid.NewGuid().ToString();

                HttpContext.Session.SetString("InstanceId", instanceId);
            }
            return instanceId;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel { AppUserId = curUserID };
            return View(createRaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {

                var race = new Race
                {
                    Date = raceVM.Date,
                    Name = raceVM.Name,
                    AddressId = raceVM.AddressId,
                    Distance = raceVM.Distance,
                    //AppUserId = raceVM.AppUserId,
                    RaceCategory = raceVM.RaceCategory,
                    Location = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    }
                };
                _UnitOfWork.RaceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _UnitOfWork.RaceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Date = race.Date,
                Name = race.Name,
                AddressId = race.AddressId,
                Address = race.Location,
                Distance = race.Distance,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View(raceVM);
            }

            var userRace = await _UnitOfWork.RaceRepository.GetByIdAsyncNoTracking(id);

            if (userRace == null)
            {
                return View("Error");
            }


            var race = new Race
            {
                Id=id,
                Date = raceVM.Date,
                Name = raceVM.Name,
                AddressId = raceVM.AddressId,
                Location = raceVM.Address,
                Distance = raceVM.Distance,
                RaceCategory = raceVM.RaceCategory
            };

            _UnitOfWork.RaceRepository.Update(race);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _UnitOfWork.RaceRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var raceDetails = await _UnitOfWork.RaceRepository.GetByIdAsync(id);



            _UnitOfWork.RaceRepository.Delete(raceDetails);
            return RedirectToAction("Index");
        }
    }
}