using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Ammunition;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Extensions;


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
            return this.HandleResponse(response);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _ammunitionService.GetById(id);
            return this.HandleResponse(response);
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
            return this.HandleResponse(response, "GetAll", "Ammunition");
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _ammunitionService.GetById(id);
            return this.HandleResponse(response);
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
            return this.HandleResponse(response, "GetAll", "Ammunition");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _ammunitionService.Delete(id);
            return this.HandleResponse(response, "GetAll", "Ammunition");
        }
    }
}
