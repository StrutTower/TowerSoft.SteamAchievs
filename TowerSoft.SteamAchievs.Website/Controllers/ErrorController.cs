using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using TowerSoft.SteamAchievs.Website.Infrastructure;

namespace TowerSoft.SteamAchievs.Website.Controllers {
    [AllowAnonymous]
    public class ErrorController : CustomController {
        public IActionResult Index() {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Exception exception = exceptionHandlerPathFeature.Error;

            if (exception is MessageException) {
                return Message(exception.Message);
            }

            if (IsAjaxRequest) {
                return Json(new {
                    errorMessage = exception.Message
                });
            }
            return View(model: exception.Message);
        }

        public IActionResult Code(int id) {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (id) {
                case (int)HttpStatusCode.Unauthorized:
                case (int)HttpStatusCode.Forbidden:
                    return RedirectToAction("Forbidden");
                case (int)HttpStatusCode.NotFound:
                    return View("NotFound", feature?.OriginalPath);
                default:
                    return Message($"Error Status Code: {id}");
            }
        }

        public IActionResult Forbidden() {
            return View();
        }

        public IActionResult NoAccess() {
            return View();
        }
    }
}