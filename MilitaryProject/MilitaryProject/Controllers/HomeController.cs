using Microsoft.AspNetCore.Mvc;
using MilitaryProject.Models;
using System.Diagnostics;

namespace MilitaryProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Status500(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> Status404()
        {
            return await Task.FromResult(View());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}