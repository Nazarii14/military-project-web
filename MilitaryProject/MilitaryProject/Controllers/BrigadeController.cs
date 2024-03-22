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

        public async Task<IActionResult> CreateBrigade(BrigadeViewModel model)
        {
            var responce = await _brigadeService.CreateBrigade(model);

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

        public async Task<IActionResult> UpdateBrigade(BrigadeViewModel model)
        {
            var responce = await _brigadeService.UpdateBrigade(model);

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

        public async Task<IActionResult> DeleteBrigade(int id)
        {
            var responce = await _brigadeService.DeleteBrigade(id);

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
