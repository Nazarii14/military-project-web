using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.MilitaryRoute;
using MilitaryProject.Extensions;

namespace MilitaryProject.Controllers
{
    public class MilitaryRouteController : Controller
    {
        private readonly IMilitaryRouteService _militaryRouteService;

        public MilitaryRouteController(IMilitaryRouteService militaryRouteService)
        {
            _militaryRouteService = militaryRouteService;
        }

        public async Task<IActionResult> GetAll()
        {
            var response = await _militaryRouteService.GetMilitaryRoutes();
            return this.HandleResponse(response);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _militaryRouteService.GetMilitaryRoute(id);
            return this.HandleResponse(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMilitaryRouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _militaryRouteService.Create(model);
                return this.HandleResponse(response, "GetAll", "MilitaryRoute");
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _militaryRouteService.GetMilitaryRoute(id);
            return this.HandleResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditMilitaryRouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _militaryRouteService.Update(model);
                return this.HandleResponse(response, "GetAll", "MilitaryRoute");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _militaryRouteService.Delete(id);
            return this.HandleResponse(response, "GetAll", "MilitaryRoute");
        }
    }
}
