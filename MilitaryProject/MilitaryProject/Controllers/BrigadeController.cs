using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Brigade;

namespace MilitaryProject.Controllers
{
    public class BrigadeController : Controller
    {
        private readonly IBrigadeService _brigadeService;

        public BrigadeController(IBrigadeService brigadeService)
        {
            _brigadeService = brigadeService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new BrigadeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrigadeViewModel model)
        {



            if (ModelState.IsValid)
            {
                var response = await _brigadeService.Create(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetAll", "Brigade");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _brigadeService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                //return RedirectToAction("Index", "Home");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBrigade(int id)
        {
            var responce = await _brigadeService.GetById(id);

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
            var response = await _brigadeService.GetById(id);

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

        public async Task<IActionResult> Update(int id)
        {
            var response = await _brigadeService.GetById(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
                return RedirectToAction("GetAll", "Brigade");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(BrigadeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _brigadeService.Update(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetAll", "Brigade");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _brigadeService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                TempData["AlertMessage"] = "Brigade deleted successfully.";
                TempData["ResponseStatus"] = "Success";
            }
            else
            {
                TempData["AlertMessage"] = response.Description;
                TempData["ResponseStatus"] = "Error";
            }

            return RedirectToAction("GetAll", "Brigade");
        }

    }
}
