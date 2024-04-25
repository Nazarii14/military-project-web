using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Weapon;
using System.Threading.Tasks;
using MilitaryProject.Extensions;


namespace MilitaryProject.Controllers
{
    public class WeaponController : Controller
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        public async Task<IActionResult> GetWeapons()
        {
            var response = await _weaponService.GetWeapons();
            return this.HandleResponse(response);
        }

        public async Task<IActionResult> GetWeapon(int id)
        {
            var response = await _weaponService.GetWeapon(id);
            return this.HandleResponse(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WeaponViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _weaponService.Create(model);
                return this.HandleResponse(response, "GetWeapons", "Weapon");
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _weaponService.GetWeapon(id);
            return this.HandleResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(WeaponViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _weaponService.Update(model);
                return this.HandleResponse(response, "GetWeapons", "Weapon");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _weaponService.Delete(id);
            return this.HandleResponse(response, "GetWeapons", "Weapon");
        }
    }
}
