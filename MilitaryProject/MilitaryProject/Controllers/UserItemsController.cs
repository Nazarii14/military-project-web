using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.UserItems;
using MilitaryProject.Domain.Response;
using MilitaryProject.Extensions;

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
            return this.HandleResponse(response);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _userItemsService.GetUserItem(id);
            return this.HandleResponse(response);
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
                return this.HandleResponse(response, "GetAll", "UserItems");
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _userItemsService.GetUserItem(id);
            return this.HandleResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserItemsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userItemsService.Update(model);
                return this.HandleResponse(response, "GetAll", "UserItems");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userItemsService.Delete(id);
            return this.HandleResponse(response, "GetAll", "UserItems");
        }
    }
}
