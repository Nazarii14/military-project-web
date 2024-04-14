using Microsoft.AspNetCore.Mvc;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Response;

namespace MilitaryProject.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult HandleResponse<T>(this Controller controller, BaseResponse<T> response, string redirectAction = null, string redirectController = null, object routeValues = null)
        {
            if (response.StatusCode == StatusCode.OK)
            {
                if (!string.IsNullOrEmpty(redirectAction) && !string.IsNullOrEmpty(redirectController))
                {
                    return controller.RedirectToAction(redirectAction, redirectController, routeValues);
                }
                else
                {
                    return controller.View(response.Data);
                }
            }
            else
            {
                controller.TempData["AlertMessage"] = response.Description;
                controller.TempData["ResponseStatus"] = "Error";
                return controller.BadRequest(response.Description);
            }
        }
    }
}
