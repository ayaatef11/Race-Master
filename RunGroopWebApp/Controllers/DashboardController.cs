﻿using Microsoft.AspNetCore.Mvc;
using RunGroop.Repository.Interfaces;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    //[Authorize]
    public class DashboardController(IUnitOfWork _UnitOfWork) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var userRaces = await _UnitOfWork.DashboardRepository.GetAllUserRaces();
            var userClubs = await _UnitOfWork.DashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }
    }
}