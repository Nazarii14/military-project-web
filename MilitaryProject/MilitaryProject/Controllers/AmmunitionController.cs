using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Ammunition;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.Enum;

namespace MilitaryProject.Controllers
{
    public class AmmunitionController : Controller
    {
        private readonly IAmmunitionService _ammunitionService;

        public AmmunitionController(IAmmunitionService ammunitionService)
        {
            _ammunitionService = ammunitionService;
        }

        public async Task<IActionResult> GetAll()
        {
            var response = await _ammunitionService.GetAll();
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

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _ammunitionService.GetById(id);
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AmmunitionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _ammunitionService.Create(model);
            if (response.StatusCode ==  Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetAll", "Ammunition");
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return View(model);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _ammunitionService.GetById(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AmmunitionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _ammunitionService.Update(model);
            if (response.StatusCode ==  Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetAll", "Ammunition");
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _ammunitionService.Delete(id);
            if (response.StatusCode ==  Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetAll", "Ammunition");
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
