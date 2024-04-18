using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Brigade;
using MilitaryProject.Extensions;


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
                return this.HandleResponse(response, "GetAll", "Brigade");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Commander")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _brigadeService.GetAll();
            return this.HandleResponse(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBrigade(int id)
        {
            var responce = await _brigadeService.GetById(id);
            return this.HandleResponse(responce);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _brigadeService.GetById(id);
            return this.HandleResponse(response);
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _brigadeService.GetById(id);
            return this.HandleResponse(response);
        }


        [HttpPost]
        public async Task<IActionResult> Update(BrigadeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _brigadeService.Update(model);
                return this.HandleResponse(response, "GetAll", "Brigade");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _brigadeService.Delete(id);
            return this.HandleResponse(response, "GetAll", "Brigade");
        }

    }
}
