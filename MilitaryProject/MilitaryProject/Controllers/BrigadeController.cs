using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Brigade;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.BLL.Services;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.ViewModels.User;
using Azure;

namespace MilitaryProject.Controllers
{
    public class BrigadeController : Controller
    {
        private readonly IBrigadeService _brigadeService;
        public BrigadeController(IBrigadeService brigadeService)
        {
            _brigadeService = brigadeService;
        }

        public async Task<IActionResult> GetBrigades()
        {
            var responce = await _brigadeService.GetBrigades();

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

        public async Task<IActionResult> GetBrigade(int id)
        {
            var responce = await _brigadeService.GetBrigade(id);

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

        public async Task<IActionResult> CreateBrigade()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrigade(BrigadeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _brigadeService.CreateBrigade(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetBrigades", "Brigade");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateBrigade(int id)
        {
            var responce = await _brigadeService.GetBrigade(id);

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

        [HttpPost]
        public async Task<IActionResult> UpdateBrigade(BrigadeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _brigadeService.UpdateBrigade(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetBrigade", "Brigade", new { id = response.Data.ID });
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteBrigade(int id)
        {
            var responce = await _brigadeService.DeleteBrigade(id);

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetBrigades", "Brigade");
            }
            else
            {
                TempData["AlertMessage"] = responce.Description;
                TempData["ResponseStatus"] = "Error";
                return View();
            }
        }

    }
}
