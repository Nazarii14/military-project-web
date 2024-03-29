using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Request;
using System.Threading.Tasks;

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

        public async Task<IActionResult> GetRequest(int id)
        {
            var response = await _requestService.GetRequest(id);

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

        public IActionResult CreateRequest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(RequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _requestService.CreateRequest(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetRequests", "Request");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateRequest(int id)
        {
            var response = await _requestService.GetRequest(id);

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
        public async Task<IActionResult> UpdateRequest(RequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _requestService.UpdateRequest(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetRequest", "Request", new { id = response.Data.ID });
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteRequest(int id)
        {
            var response = await _requestService.DeleteRequest(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetRequests", "Request");
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
