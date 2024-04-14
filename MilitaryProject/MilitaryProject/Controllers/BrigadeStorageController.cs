using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.BrigadeStorage;
using System.Security.Claims;
using MilitaryProject.Extensions;


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
                return this.HandleResponse(response, "GetBrigadeStorages", "BrigadeStorage");
            }
            return View(brigadeStorage);
        }

        [HttpGet]
        public async Task<IActionResult> GetBrigadeStorages()
        {
            var response = await _brigadeStorageService.GetAll();
            return this.HandleResponse(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBrigadeStorage(int id)
        {
            var response = await _brigadeStorageService.GetById(id);
            return this.HandleResponse(response);
        }

        [HttpGet]
        public async Task<IActionResult> EditBrigadeStorage(int id)
        {
            var response = await _brigadeStorageService.GetById(id);
            return this.HandleResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBrigadeStorage(BrigadeStorageViewModel brigadeStorage)
        {
            if (ModelState.IsValid)
            {
                var response = await _brigadeStorageService.Update(brigadeStorage);
                return this.HandleResponse(response, "GetBrigadeStorages", "BrigadeStorage");
            }
            return View(brigadeStorage);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrigadeStorage(int id)
        {
            var response = await _brigadeStorageService.Delete(id);
            return this.HandleResponse(response, "GetBrigadeStorages", "BrigadeStorage");
        }
    }
}
