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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MilitaryProject.Controllers
{
    public class WeaponController : Controller
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpGet]
        public IActionResult CreateWeapon()
        {
            return View(new WeaponViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeapon(WeaponViewModel model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _weaponService.Create(model);
                if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertMessage"] = responce.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetWeapons()
        {
            var responce = await _weaponService.GetAll();
            
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

        [HttpGet]
        public async Task<IActionResult> GetWeapon(int id)
        {
            var responce = await _weaponService.GetById(id);
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var responce = await _weaponService.GetById(id);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var responce = await _weaponService.GetById(id);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateWeapon(WeaponViewModel model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _weaponService.Update(model);
                if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertMessage"] = responce.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWeapon(int id)
        {
            var responce = await _weaponService.Delete(id);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                TempData["AlertMessage"] = "Weapon deleted successfully.";
                TempData["ResponseStatus"] = "Success";
            }
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
