using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.UserItems;
using MilitaryProject.Domain.Response;

namespace MilitaryProject.Controllers
{
    public class UserItemsController : Controller
    {
        private readonly IUserItemsService _userItemsService;

        public UserItemsController(IUserItemsService userItemsService)
        {
            _userItemsService = userItemsService;
        }

        public async Task<IActionResult> GetAll()
        {
            var response = await _userItemsService.GetUserItems();

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
            var response = await _userItemsService.GetUserItem(id);

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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserItemsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userItemsService.Create(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetAll", "UserItems");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _userItemsService.GetUserItem(id);

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
        public async Task<IActionResult> Update(UserItemsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userItemsService.Update(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetAll", "UserItems", new { id = response.Data.ID });
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
            var response = await _userItemsService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetAll", "UserItems");
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
