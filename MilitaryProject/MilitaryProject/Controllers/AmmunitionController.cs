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

        public async Task<IActionResult> Index()
        {
            var response = await _ammunitionService.GetAllAmmunition();
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

        public async Task<IActionResult> Details(int id)
        {
            var response = await _ammunitionService.GetAmmunitionById(id);
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

            var response = await _ammunitionService.CreateAmmunition(model);
            if (response.StatusCode ==  Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _ammunitionService.GetAmmunitionById(id);
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
        public async Task<IActionResult> Edit(AmmunitionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _ammunitionService.UpdateAmmunition(model);
            if (response.StatusCode ==  Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
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
            var response = await _ammunitionService.GetAmmunitionById(id);
            if (response.StatusCode ==  Domain.Enum.StatusCode.OK)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _ammunitionService.DeleteAmmunition(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return BadRequest(response.Description);
            }
        }
    }
}
