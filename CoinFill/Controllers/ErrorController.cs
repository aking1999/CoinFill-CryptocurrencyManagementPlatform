using CoinFill.Emails;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CoinFill.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(IErrorLogger errors,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager) : base(errors, mailService, contextAccessor, notificationRepository, userManager)
        {

        }

        [Route("/error/{statusCode}")]
        public async Task<IActionResult> HttpErrorHandler(int statusCode)
        {
            var statusCodeReExecuteResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteResult == null) return RedirectToAction("Index", "Home", new { Area = "" });

            switch (statusCode)
            {
                case 403:
                    {
                        await _errors.SaveErrorAsync($"Error 403: Forbidden access to {statusCodeReExecuteResult.OriginalPath ?? ""}.", null, null, null);
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return View("~/Views/Shared/ErrorPages/Forbidden403.cshtml");
                    }
                case 404:
                    {
                        await _errors.SaveErrorAsync($"Error 404: Resource {statusCodeReExecuteResult.OriginalPath ?? ""} not found.", null, null, null);
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return View("~/Views/Shared/ErrorPages/NotFound404.cshtml",
                            new PartialViewModels.NotFoundPartialViewModel
                            {
                                HolderClasses = "mt-3",
                                Title = "Requested page not found.",
                                Body = "The page you are looking for does not exist or has been renamed.",
                                IncludeSupportInfo = true,
                                PrimaryButtonText = "<i class='fas fa-home mr-2'></i>Home",
                                PrimaryButtonClasses = "btn btn-primary btn-block mt-3",
                                PrimaryButtonUrl = Url.Action("Index", "Home")
                            });
                    }
                default:
                    {
                        var exceptionResult = HttpContext.Features.Get<IExceptionHandlerFeature>();
                        var exceptionPathResult = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                        var errorMessage = string.Empty;

                        if (statusCodeReExecuteResult != null && !string.IsNullOrWhiteSpace(statusCodeReExecuteResult.OriginalPath))
                            errorMessage += $"Error {statusCode}: Resource {statusCodeReExecuteResult.OriginalPath} caused an error. ";

                        if (exceptionPathResult != null && !string.IsNullOrWhiteSpace(exceptionPathResult.Path))
                            errorMessage += $"Error {statusCode}: Resource {exceptionPathResult.Path} caused an error.";

                        if (exceptionResult != null && exceptionResult.Error != null)
                        {
                            await _errors.SaveErrorAsync(exceptionResult.Error, null, null, null);
                        }

                        if (exceptionPathResult != null && exceptionPathResult.Error != null)
                        {
                            await _errors.SaveErrorAsync(exceptionPathResult.Error, null, null, null);
                        }

                        if (!string.IsNullOrWhiteSpace(errorMessage))
                            await _errors.SaveErrorAsync(errorMessage, null, null, null);

                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return View("~/Views/Shared/ErrorPages/InternalServerError500.cshtml");
                    }
            }
        }

        [Route("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            if (!TempData.ContainsKey("isSearchedByUrl") || (bool)TempData["isSearchedByUrl"])
                return RedirectToAction("Index", "Home", new { Area = "" });

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("~/Views/Shared/ErrorPages/InternalServerError500.cshtml");
        }

        [Route("/error/error-response")]
        public async Task<IActionResult> UnhandledError()
        {
            var exceptionResult = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionResult == null) return RedirectToAction("Index", "Home", new { Area = "" });

            await _errors.SaveErrorAsync(exceptionResult.Error, null, null, null);

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("~/Views/Shared/ErrorPages/InternalServerError500.cshtml");
        }

        [HttpPost("/error/log-front-end-error")]
        public async Task LogFrontEndErrors(string errorText)
        {
            var normalizedErrorText = errorText.ToLower();

            var length = normalizedErrorText.Length;

            if (normalizedErrorText[length - 3] == '[' &&
               normalizedErrorText[length - 2] == '~' &&
               normalizedErrorText[length - 1] == ']' &&
               normalizedErrorText.Contains("url") &&
               normalizedErrorText.Contains("line") &&
               normalizedErrorText.Contains("column") &&
               normalizedErrorText.Contains("stack"))
                await HandleErrorAsync(errorText);
            return;
        }

        [Route("/error/access-denied")]
        public async Task<IActionResult> AccessDenied()
        {
            await _errors.SaveErrorAsync($"Error 403: Forbidden access to {HttpContext?.Request.PathBase.Value ?? ""}.", null, null, null);
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View("~/Views/Shared/ErrorPages/Forbidden403.cshtml");
        }
    }
}
