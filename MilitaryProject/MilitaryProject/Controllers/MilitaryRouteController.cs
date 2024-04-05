using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.MilitaryRoute;

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
            var response = await _militaryRouteService.GetMilitaryRoute(id);

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
        public async Task<IActionResult> Create(CreateMilitaryRouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _militaryRouteService.Create(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    TempData["ResponseStatus"] = "Success";
                    return RedirectToAction("GetAll", "MilitaryRoute");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                    return BadRequest(response.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _militaryRouteService.GetMilitaryRoute(id);

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

        [HttpPost]
        public async Task<IActionResult> Update(EditMilitaryRouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _militaryRouteService.Update(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    TempData["ResponseStatus"] = "Success";
                    return RedirectToAction("GetAll", "MilitaryRoute", new { id = response.Data.ID });
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                    return BadRequest(response.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _militaryRouteService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                TempData["ResponseStatus"] = "Success";
                return RedirectToAction("GetAll", "MilitaryRoute");
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
