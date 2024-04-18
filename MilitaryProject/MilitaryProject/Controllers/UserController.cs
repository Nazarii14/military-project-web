using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.Domain.ViewModels.User;
using System.Security.Claims;

namespace MilitaryProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController (IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new SignupViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.CheckUserExistence(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK && response.Data == null)
                {
                    return RedirectToAction("SignupTwoFA", "User", model);
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> SignupTwoFA(SignupViewModel model)
        {
            var response = await _userService.QrCode(model);
            model.QrCode = response.Data;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TwoFASignup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.SignUp(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data.Claims));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.CheckCreds(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK && response.Data != null)
                {
                    return RedirectToAction("LoginTwoFA", "User", model);
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> LoginTwoFA(LoginViewModel model)
        {
            var response = await _userService.QrCode(model);
            model.QrCode = response.Data;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TwoFALogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.Login(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data.Claims));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertMessage"] = response.Description;
                    TempData["ResponseStatus"] = "Error";
                }
            }
            return View(model);
        }


        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
