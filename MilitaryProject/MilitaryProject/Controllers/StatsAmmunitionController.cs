using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Stats;
using MilitaryProject.Extensions;
using System.Security.Claims;

namespace MilitaryProject.Controllers
{
    public class StatsAmmunitionController : Controller
    {
        private readonly IStatsAmmunitionService _statsAmmunitionService;

        public StatsAmmunitionController(IStatsAmmunitionService statsAmmunitionService)
        {
            _statsAmmunitionService = statsAmmunitionService;
        }

        [Authorize(Roles = "Admin, Commander, Soldier")]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            if (userRole == "Soldier")
            {
                var response = await _statsAmmunitionService.GetAll(userId);
                return View("GetAllForSoldier", response.Data);
            }
            else
            {
                var response = await _statsAmmunitionService.GetAll(userId);
                return this.HandleResponse(response);
            }
        }

        [Authorize(Roles = "Admin, Commander")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Create(StatsAmmunitionViewModel model)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);
            model.UserID = userId;

            if (ModelState.IsValid)
            {
                var response = await _statsAmmunitionService.Create(model);
                return this.HandleResponse(response, "GetAll", "StatsAmmunition");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Update(int ammunitionID)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);

            var response = await _statsAmmunitionService.GetById(userId, ammunitionID);
            return this.HandleResponse(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Update(StatsAmmunitionViewModel model)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);
            model.UserID = userId;

            if (ModelState.IsValid)
            {
                var response = await _statsAmmunitionService.Update(model);
                return this.HandleResponse(response, "GetAll", "StatsAmmunition");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Delete(int ammunitionID)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);

            var response = await _statsAmmunitionService.Delete(userId, ammunitionID);
            return this.HandleResponse(response, "GetAll", "StatsAmmunition");
        }

        [Authorize(Roles = "Admin, Commander, Soldier")]
        public async Task<IActionResult> GetById(int ammunitionID)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            if (userRole == "Soldier")
            {
                var response = await _statsAmmunitionService.GetById(userId, ammunitionID);
                return View("GetForSoldier", response.Data);
            }
            else
            {
                var response = await _statsAmmunitionService.GetById(userId, ammunitionID);
                return this.HandleResponse(response);
            }
        }
    }
}
