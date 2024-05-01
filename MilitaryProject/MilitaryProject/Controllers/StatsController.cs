using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.Stats;
using System.Threading.Tasks;
using MilitaryProject.Extensions;
using System.Security.Claims;

namespace MilitaryProject.Controllers
{
    public class StatsController : Controller
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> ViewStats()
        {
            //foreach (var claim in User.Claims)
            //{
            //    Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            //}

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(userIdClaim.Value);

            var response = await _statsService.GetBrigadeStatistics(userId);
            return View(response.Data);
        }
    }
}
