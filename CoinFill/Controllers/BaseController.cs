using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Implementations;
using CoinFill.Helpers.Models;
using CoinFill.Helpers.Extensions;
using CoinFill.Emails;
using CoinFill.Notifications;

namespace CoinFill.Controllers
{
    public class BaseController : Controller
    {
        private ICompositeViewEngine _viewEngine => HttpContext.RequestServices.GetRequiredService<ICompositeViewEngine>();

        protected readonly ISession _session;
        protected readonly IErrorLogger _errors;
        protected readonly IMailService _mailService;
        protected readonly INotificationRepository _notificationRepository;
        protected readonly CoinFillContext _context;
        protected readonly UserManager<CustomClient> _userManager;
        protected readonly SignInManager<CustomClient> _signInManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        private string AREA_NAME => HttpContext?.GetRouteData()?.Values["area"]?.ToString();
        private string CONTROLLER_NAME => HttpContext?.GetRouteValue("controller")?.ToString();
        private string ACTION_NAME => HttpContext?.GetRouteValue("action")?.ToString();

        protected BaseController(IErrorLogger error,
            IHttpContextAccessor contextAccessor)
        {
            _session = contextAccessor.HttpContext.Session;
            _errors = error;
        }

        protected BaseController(IErrorLogger error,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager,
            SignInManager<CustomClient> signInManager)
        {
            _session = contextAccessor.HttpContext.Session;
            _errors = error;
            _mailService = mailService;
            _notificationRepository = notificationRepository;
            _context = new CoinFillContext();
            _userManager = userManager;
            _signInManager = signInManager;
        }

        protected BaseController(IErrorLogger errors,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager)
        {
            _session = contextAccessor.HttpContext.Session;
            _errors = errors;
            _mailService = mailService;
            _notificationRepository = notificationRepository;
            _context = new CoinFillContext();
            _userManager = userManager;
        }

        protected BaseController(IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<CustomClient> signInManager)
        {
            _session = contextAccessor.HttpContext.Session;
            _errors = new ErrorLogger(contextAccessor);
            _mailService = mailService;
            _notificationRepository = notificationRepository;
            _context = new CoinFillContext();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [NonAction]
        protected void ShowToastOnThisPageIfSet()
        {
            if (_session.HasToast())
            {
                ViewBag.toast = _session.GetToast();
                _session.RemoveToastFromKeys();
            }
        }

        [NonAction]
        protected async Task<RedirectToActionResult> HandleErrorAsync(string errorMessage)
        {
            try
            {
                await _errors.SaveErrorAsync(errorMessage, AREA_NAME, CONTROLLER_NAME, ACTION_NAME);
            }
            catch (Exception e)
            {
                _errors.SaveError(e, "CoinFill", "BaseController", "HandleErrorAsync");
            }

            TempData["isSearchedByUrl"] = false;
            return RedirectToAction("Error", "Error", new { Area = "" });
        }

        [NonAction]
        protected async Task<JsonResult> HandleErrorJsonAsync(string errorMessage)
        {
            try
            {
                await _errors.SaveErrorAsync(errorMessage, AREA_NAME, CONTROLLER_NAME, ACTION_NAME);
            }
            catch (Exception e)
            {
                _errors.SaveError(e, "CoinFill", "BaseController", "HandleErrorJsonAsync");
            }

            TempData["isSearchedByUrl"] = false;
            return Json(new
            {
                success = false,
                redirectUrl = Url.Action("Error", "Error", new { Area = "" })
            });
        }

        [NonAction]
        protected async Task<RedirectToActionResult> HandleErrorAsync(Exception e)
        {
            var signOutUser = false;

            if (e.GetType() == typeof(GeneralException))
                signOutUser = ((GeneralException)e).SignOutUser;

            try
            {
                await _errors.SaveErrorAsync(e, AREA_NAME, CONTROLLER_NAME, ACTION_NAME);

                if (signOutUser && User.Identity.IsAuthenticated) await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                _errors.SaveError(ex, "CoinFill", "BaseController", "HandleErrorAsync");

                if (signOutUser && User.Identity.IsAuthenticated) _signInManager.SignOutAsync().Wait();
            }

            TempData["isSearchedByUrl"] = false;
            return RedirectToAction("Error", "Error", new { Area = "" });
        }

        [NonAction]
        protected async Task<JsonResult> HandleErrorJsonAsync(Exception e)
        {
            var signOutUser = false;

            if (e.GetType() == typeof(GeneralException))
                signOutUser = ((GeneralException)e).SignOutUser;

            try
            {
                await _errors.SaveErrorAsync(e, AREA_NAME, CONTROLLER_NAME, ACTION_NAME);

                if (signOutUser && User.Identity.IsAuthenticated) await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                _errors.SaveError(ex, "CoinFill", "BaseController", "HandleErrorJsonAsync");

                if (signOutUser && User.Identity.IsAuthenticated) _signInManager.SignOutAsync().Wait();
            }

            TempData["isSearchedByUrl"] = false;
            return Json(new
            {
                success = false,
                redirectUrl = Url.Action("Error", "Error", new { Area = "" })
            });
        }

        protected async Task<string> RenderPartialViewToStringAsync(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}