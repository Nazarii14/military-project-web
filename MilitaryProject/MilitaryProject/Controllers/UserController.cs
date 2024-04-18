using Castle.Core.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.ViewModels.User;
using System.Security.Claims;

namespace MilitaryProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController (IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
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

        public async Task<IActionResult> RestorePassword()
        {
            return await Task.FromResult(View());
        }

        //[HttpPost]
        //public async Task<IActionResult> RestorePassword(RestorePasswordViewModel model)
        //{
        //    var user = await _userService.GetUser(model.Email);

        //    if (user.Data != null)
        //    {
        //        var token = await _userService.GenerateResetToken(user.Data.Email);
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Data.ID, token.Data },
        //            protocol: HttpContext.Request.Scheme);

        //        await _emailService.SendEmailAsync(
        //            model.Email,
        //            "Reset Password",
        //            $"Reset your password by clicking here: <a href='{callbackUrl}'>link</a>",
        //            "classroom0570@gmail.com");
        //    }

        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> RestorePassword(RestorePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.ChangePassword(model);

                if (response.StatusCode == Domain.Enum.StatusCode.OK && response.Data == true)
                {
                    return RedirectToAction("Login", "User");
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
