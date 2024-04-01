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

        public async Task<IActionResult> GetUserItems()
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

        public async Task<IActionResult> GetUserItem(int id)
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

        public async Task<IActionResult> CreateUserItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserItem(UserItemsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userItemsService.CreateUserItems(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetUserItems", "UserItems");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateUserItem(int id)
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
        public async Task<IActionResult> UpdateUserItem(UserItemsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userItemsService.UpdateUserItems(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetUserItem", "UserItems", new { id = response.Data.ID });
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteUserItem(int id)
        {
            var response = await _userItemsService.DeleteUserItems(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetUserItems", "UserItems");
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
