using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Request;
using System.Threading.Tasks;
using MilitaryProject.Extensions;

namespace MilitaryProject.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<IActionResult> GetRequests()
        {
            var response = await _requestService.GetRequests();
            return this.HandleResponse(response);
        }

        public async Task<IActionResult> GetRequest(int id)
        {
            var response = await _requestService.GetRequest(id);
            return this.HandleResponse(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _requestService.Create(model);
                return this.HandleResponse(response, "GetRequests", "Request");
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _requestService.GetRequest(id);
            return this.HandleResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _requestService.Update(model);
                return this.HandleResponse(response, "GetRequests", "Request");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _requestService.Delete(id);
            return this.HandleResponse(response, "GetRequests", "Request");
        }
    }
}
