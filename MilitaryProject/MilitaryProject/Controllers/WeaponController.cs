using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Weapon;
using System.Threading.Tasks;
using MilitaryProject.Extensions;
using Microsoft.AspNetCore.Authorization;


namespace MilitaryProject.Controllers
{
    public class WeaponController : Controller
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> GetWeapons()
        {
            var response = await _weaponService.GetWeapons();
            return this.HandleResponse(response);
        }

        [Authorize(Roles = "Soldier")]
        public async Task<IActionResult> GetWeaponsForSoldier()
        {
            var response = await _weaponService.GetWeapons();
            return View("GetWeaponsForSoldier", response.Data);
        }

        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> GetWeapon(int id)
        {
            var response = await _weaponService.GetWeapon(id);
            return this.HandleResponse(response);
        }

        [Authorize(Roles = "Soldier")]
        public async Task<IActionResult> GetWeaponForSoldier(int id)
        {
            var response = await _weaponService.GetWeapon(id);
            return View("GetWeaponForSoldier", response.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Create(WeaponViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _weaponService.Create(model);
                return this.HandleResponse(response, "GetWeapons", "Weapon");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Update(int id)
        {
            var response = await _weaponService.GetWeapon(id);
            return this.HandleResponse(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Update(WeaponViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _weaponService.Update(model);
                return this.HandleResponse(response, "GetWeapons", "Weapon");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _weaponService.Delete(id);
            return this.HandleResponse(response, "GetWeapons", "Weapon");
        }
    }
}
