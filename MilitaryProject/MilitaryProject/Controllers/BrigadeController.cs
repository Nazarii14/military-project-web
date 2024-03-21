using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Brigade;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.BLL.Services;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.Enum;

namespace MilitaryProject.Controllers
{
    public class BrigadeController : Controller
    {
        private readonly IBrigadeService _brigadeService;
        private readonly IMapper _mapper;
        private readonly BrigadeRepository _brigadeRepository;
        public BrigadeController(IBrigadeService brigadeService)
        {
            _brigadeService = brigadeService;
        }

        public async Task<BaseResponse<ReadBrigadeViewModel>> Read(int id)
        {
            var response = new BaseResponse<ReadBrigadeViewModel>();

            try
            {
                var brigade = await _brigadeRepository.GetById(id);
                if (brigade != null)
                {
                    var brigadeViewModel = _mapper.Map<ReadBrigadeViewModel>(brigade);
                    response.Data = brigadeViewModel;
                    response.StatusCode = Domain.Enum.StatusCode.OK;
                }
                else
                {
                    response.Description = $"Brigade with ID {id} not found.";
                    response.StatusCode = Domain.Enum.StatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.Description = "Error occurred while retrieving brigade.";
                response.StatusCode = Domain.Enum.StatusCode.InternalServerError;
            }

            return response;
        }

        public IActionResult Create()
        {
            var createBrigadeViewModel = new CreateBrigadeViewModel();
            return View(createBrigadeViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateBrigadeViewModel createBrigadeViewModel)
        {
            if (ModelState.IsValid)
            {
                _brigadeService.Create(createBrigadeViewModel);
                return RedirectToAction("Index");
            }
            return View(createBrigadeViewModel);
        }

        public IActionResult Edit(int id)
        {
            var brigadeViewModel = _brigadeService.GetById(id);
            if (brigadeViewModel == null)
            {
                return NotFound();
            }
            return View(brigadeViewModel);
        }

        public IActionResult Delete(int id)
        {
            var brigadeViewModel = _brigadeService.GetById(id);
            if (brigadeViewModel == null)
            {
                return NotFound();
            }
            return View(brigadeViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            _brigadeService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
