using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Weapon;
using System.Threading.Tasks;

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

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(response.Description);
            }
        }

        public async Task<IActionResult> GetWeapon(int id)
        {
            var response = await _weaponService.GetWeapon(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(response.Description);
            }
        }

        public IActionResult CreateWeapon()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeapon(WeaponViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _weaponService.CreateWeapon(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetWeapons", "Weapon");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateWeapon(int id)
        {
            var response = await _weaponService.GetWeapon(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {

                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(response.Description);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWeapon(WeaponViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _weaponService.UpdateWeapon(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetWeapons", "Weapon", new { id = response.Data.ID });
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteWeapon(int id)
        {
            var response = await _weaponService.DeleteWeapon(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetWeapons", "Weapon");
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return View();
            }
        }
    }
}
