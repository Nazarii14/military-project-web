using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Weapon;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.BLL.Services;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.Enum;
using AutoMapper;
using MilitaryProject.Domain.ViewModels.User;
using Azure;

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
            var responce = await _weaponService.GetWeapons();

            
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }
            
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(responce.Description);
            }
        }

        public async Task<IActionResult> GetWeapon(int id)
        {
            var responce = await _weaponService.GetWeapon(id);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(responce.Description);
            }
        }

        public async Task<IActionResult> CreateWeapon(WeaponViewModel model)
        {
            var responce = await _weaponService.CreateWeapon(model);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(responce.Description);
            }
        }

        public async Task<IActionResult> UpdateWeapon(WeaponViewModel model)
        {
            var responce = await _weaponService.UpdateWeapon(model);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(responce.Description);
            }
        }

        public async Task<IActionResult> DeleteWeapon(int id)
        {
            var responce = await _weaponService.DeleteWeapon(id);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(responce.Description);
            }
        }
    }
}
