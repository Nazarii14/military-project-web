using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Stats;
using MilitaryProject.Extensions;
using System.Security.Claims;

namespace MilitaryProject.Controllers
{
    public class StatsWeaponController : Controller
    {
        private readonly IStatsWeaponService _statsWeaponService;

        public StatsWeaponController(IStatsWeaponService statsWeaponService)
        {
            _statsWeaponService = statsWeaponService;
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
                var response = await _statsWeaponService.GetAll(userId);
                return View("GetAllForSoldier", response.Data);
            }
            else
            {
                var response = await _statsWeaponService.GetAll(userId);
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
        public async Task<IActionResult> Create(StatsWeaponViewModel model)
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
                var response = await _statsWeaponService.Create(model);
                return this.HandleResponse(response, "GetAll", "StatsWeapon");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Update(int weaponID)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);

            var response = await _statsWeaponService.GetById(userId, weaponID);
            return this.HandleResponse(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Update(StatsWeaponViewModel model)
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
                var response = await _statsWeaponService.Update(model);
                return this.HandleResponse(response, "GetAll", "StatsWeapon");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Delete(int weaponID)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);

            var response = await _statsWeaponService.Delete(userId, weaponID);
            return this.HandleResponse(response, "GetAll", "StatsWeapon");
        }

        [Authorize(Roles = "Admin, Commander, Soldier")]
        public async Task<IActionResult> GetById(int weaponID)
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
                var response = await _statsWeaponService.GetById(userId, weaponID);
                return View("GetForSoldier", response.Data);
            }
            else
            {
                var response = await _statsWeaponService.GetById(userId, weaponID);
                return this.HandleResponse(response);
            }
        }
    }
}
