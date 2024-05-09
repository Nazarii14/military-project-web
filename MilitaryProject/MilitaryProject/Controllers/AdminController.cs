using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.BLL.Services;
using MilitaryProject.Domain.ViewModels.Admin;
using MilitaryProject.Extensions;

namespace MilitaryProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBrigadeService _brigadeService;
        private readonly IRequestService _requestService;

        public AdminController(IUserService userService, IBrigadeService brigadeService, IRequestService requestService)
        {
            _userService = userService;
            _brigadeService = brigadeService;
            _requestService = requestService;
        }

        public async Task<IActionResult> AdminPannel()
        {
            var users = await _userService.GetAll();
            var brigades = await _brigadeService.GetAll();
            var requests = await _requestService.GetRequests();

            var adminModel = new AdminPannelViewModel
            {
                Users = users.Data,
                Brigades = brigades.Data,
                Requests = requests.Data
            };

            return View(adminModel);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id);
            return this.HandleResponse(response, "AdminPannel", "Admin");
        }

        public async Task<IActionResult> GetUser(int id)
        {
            var response = await _userService.GetUser(id);
            return this.HandleResponse(response);
        }
    }
}
