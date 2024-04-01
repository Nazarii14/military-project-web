using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.BrigadeStorage;
using System.Security.Claims;


namespace MilitaryProject.Controllers
{
    public class BrigadeStorageController : Controller
    {
        public IBrigadeStorageService _brigadeStorageService;

        public BrigadeStorageController(IBrigadeStorageService brigadeStorageService)
        {
            _brigadeStorageService = brigadeStorageService;
        }

        [HttpGet]
        public IActionResult CreateBrigadeStorage()
        {
            return View(new BrigadeStorageViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrigadeStorage(BrigadeStorageViewModel brigadeStorage)
        {
            if (ModelState.IsValid)
            {
                var response = await _brigadeStorageService.Create(brigadeStorage);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(brigadeStorage);
        }

        [HttpGet]
        public async Task<IActionResult> GetBrigadeStorages()
        {
            var response = await _brigadeStorageService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBrigadeStorage(int id)
        {
            var response = await _brigadeStorageService.GetById(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBrigadeStorage(int id)
        {
            var response = await _brigadeStorageService.GetById(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBrigadeStorage(BrigadeStorageViewModel brigadeStorage)
        {
            if (ModelState.IsValid)
            {
                var response = await _brigadeStorageService.Update(brigadeStorage);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(brigadeStorage);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrigadeStorage(int id)
        {
            var response = await _brigadeStorageService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                TempData["AlertMessage"] = "BrigadeStorage deleted successfully.";
                TempData["ResponseStatus"] = "Success";
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
