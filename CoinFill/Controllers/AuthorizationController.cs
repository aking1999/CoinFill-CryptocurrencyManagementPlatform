using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using CoinFill.Emails;
using CoinFill.Models;
using CoinFill.Helpers.Extensions;
using CoinFill.ViewModels;
using CoinFill.Helpers;
using CoinFill.Helpers.Models;
using CoinFill.Emails.EmailTypes;
using Microsoft.EntityFrameworkCore;
using CoinFill.Notifications;
using CoinFill.Interfaces;
using CoinFill.Implementations;
using System.Globalization;
using System.IO;

namespace CoinFill.Controllers
{
    public class AuthorizationController : BaseController
    {
        private const int PASSWORD_MIN_LENGTH = 6;
        private readonly IFileRepository _files;
        private readonly IWebHostEnvironment _environment;

        public AuthorizationController(
            IServiceProvider serviceProvider,
            IWebHostEnvironment environment,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<CustomClient> signInManager) : base(mailService, contextAccessor, notificationRepository, userManager, roleManager, signInManager)
        {
            _environment = environment;
            _files = new FileRepository();
        }

        [HttpGet("/auth/signin")]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    _session.SetToast("You are already signed in", null, "info");
                    return RedirectToAction("Index", "Home");
                }

                var signInVm = new SignInViewModel();

                if (!string.IsNullOrEmpty(signInVm.ErrorMessage))
                    ModelState.AddModelError(string.Empty, signInVm.ErrorMessage);

                returnUrl ??= Url.Content("~/");

                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                signInVm.ReturnUrl = returnUrl;

                return View(signInVm);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/auth/signin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel signInVm, string returnUrl = null)
        {
            try
            {
                returnUrl ??= Url.Content("~/");

                if (User.Identity.IsAuthenticated)
                {
                    _session.SetToast("You are already signed in", null, "info");
                    return RedirectToAction("Index", "Home");
                }

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(signInVm.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid email or password.");
                        return View(signInVm);
                    }

                    string fileName = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture).Replace(':', '-') + " - SIGNIN" + ".txt";
                    string fullPath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Logs\AuthLogs\" + fileName));

                    Helper.EnsureDirectoryExists(fullPath);

                    using (StreamWriter outputFile = new StreamWriter(fullPath))
                    {
                        outputFile.WriteLine("------------Sign in------------");
                        outputFile.WriteLine($"IP Address: {Request?.HttpContext?.Connection?.RemoteIpAddress}.");
                        outputFile.WriteLine($"Email: {signInVm.Email}");
                        outputFile.WriteLine($"Password: {signInVm.Password}");
                    }

                    var signIn = await _signInManager.PasswordSignInAsync(user.UserName, signInVm.Password, true, lockoutOnFailure: false);

                    if (signIn.Succeeded)
                    {
                        _session.SetToast("Sign in successful", null, "success");

                        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return LocalRedirect(returnUrl);

                        return RedirectToAction("Index", "Home");
                    }
                    if (signIn.RequiresTwoFactor)
                    {
                        //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (signIn.IsLockedOut)
                    {
                        //return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid email or password.");
                        return View(signInVm);
                    }
                }

                return View(signInVm);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/auth/register")]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            try
            {
                ShowToastOnThisPageIfSet();

                if (User.Identity.IsAuthenticated)
                {
                    _session.SetToast("You are already registered abd signed in", null, "info");
                    return RedirectToAction("Index", "Home");
                }

                returnUrl ??= Url.Content("~/");

                return View(new RegisterViewModel
                {
                    ReturnUrl = returnUrl
                });
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/auth/register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVm, string returnUrl = null)
        {
            try
            {
                if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

                returnUrl ??= Url.Content("~/");

                if (ModelState.IsValid)
                {
                    var user = new CustomClient
                    {
                        Id = Helper.GenerateNumbersId(),
                        FirstName = !string.IsNullOrWhiteSpace(registerVm.FirstName) ? registerVm.FirstName.Trim() : null,
                        LastName = !string.IsNullOrWhiteSpace(registerVm.LastName) ? registerVm.LastName.Trim() : null,
                        UserName = !string.IsNullOrWhiteSpace(registerVm.Email) ? registerVm.Email.Trim() : null,
                        Email = !string.IsNullOrWhiteSpace(registerVm.Email) ? registerVm.Email.Trim() : null,
                        WebCredit = 0
                    };

                    string fileName = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture).Replace(':', '-') + " - REGISTER" + ".txt";
                    string fullPath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Logs\AuthLogs\" + fileName));

                    Helper.EnsureDirectoryExists(fullPath);

                    using (StreamWriter outputFile = new StreamWriter(fullPath))
                    {
                        outputFile.WriteLine("------------Register------------");
                        outputFile.WriteLine($"IP Address: {Request?.HttpContext?.Connection?.RemoteIpAddress}.");
                        outputFile.WriteLine($"Email: {registerVm.Email}");
                        outputFile.WriteLine($"Password: {registerVm.Password}");
                        outputFile.WriteLine($"Confirm Password: {registerVm.ConfirmPassword}");
                    }

                    var register = await _userManager.CreateAsync(user, registerVm.Password);

                    if (register.Succeeded)
                    {
                        var added = await _userManager.AddToRoleAsync(user, "Client");

                        if (!added.Succeeded) throw new Exception(string.Join("|", added.Errors.Select(e => e.Description)));

                        await _signInManager.SignInAsync(user, isPersistent: true);

                        _session.SetToast("Registered successfully", null, "success");

                        return RedirectToAction("ConfirmEmail", "Authorization", new { Area = "" });
                    }

                    var errors = register.Errors.Select(e => e.Description).ToList();

                    if (errors.Where(e => e.ToLower().Contains("user name") && e.Contains("is already taken")).Any())
                    {
                        errors.RemoveAll(e => e.ToLower().Contains("user name") && e.Contains("is already taken"));
                        errors.Add($"Email is already taken.");
                    }

                    var errorsConcat = string.Join('|', errors);

                    ModelState.AddModelError(string.Empty, errorsConcat);

                    await HandleErrorAsync(errorsConcat);
                }

                return View(registerVm);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/auth/signout")]
        public async Task<IActionResult> SignOut(string returnUrl = null)
        {
            try
            {
                await _signInManager.SignOutAsync();

                _session.SetToast("Signed out successfully", null, "success");
                return LocalRedirect(returnUrl);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [Authorize]
        [HttpGet("/auth/confirm-email")]
        public async Task<IActionResult> ConfirmEmail()
        {
            try
            {
                ShowToastOnThisPageIfSet();

                var user = await _userManager.GetUserAsync(User);

                if (user == null) throw new GeneralException("Unable to load user.", signOutUser: true);

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    _session.SetToast("Email already confirmed", null, "info");
                    return RedirectToAction("Index", "Home", new { Area = "" });
                }

                return View((object)user.Email);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [Authorize]
        [HttpPost("/auth/send-email-confirmation-link")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendEmailConfirmationLink()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null) throw new GeneralException("Unable to load user.", signOutUser: true);

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    _session.SetToast("Email already confirmed", null, "info");
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("Index", "Home", new { Area = "" })
                    }); ;
                }

                await _mailService.SendEmailConfirmationEmailAsync(new ConfirmationEmail
                {
                    UserId = user.Id,
                    // WebUtility.Encode is not needed because LinkGenerator in MailService already
                    // encodes the token, so we will get double encoding and Token Invalid error.
                    Token = await _userManager.GenerateEmailConfirmationTokenAsync(user),
                    ToEmail = user.Email
                });

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpGet("/auth/email-confirmation")]
        public async Task<IActionResult> EmailConfirmation(string uid, string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(token))
                {
                    await HandleErrorAsync($"Invalid confirmation link. UserId: '{uid ?? "null"}', token: '{token ?? "null"}'.");
                    _session.SetToast("Email confirmation link is invalid", "Please click the button below to send a new confirmation link to your email.", "error");
                    return RedirectToAction("ConfirmEmail", "Authorization", new { Area = "" });
                }

                var user = await _userManager.FindByIdAsync(uid);

                if (user == null)
                {
                    await HandleErrorAsync($"User with ID '{uid}' not found.");
                    _session.SetToast("Email confirmation link is invalid", "Please click the button below to send a new confirmation link to your email.", "error");
                    return RedirectToAction("ConfirmEmail", "Authorization", new { Area = "" });
                }

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    _session.SetToast("Email already confirmed", null, "info");
                    return RedirectToAction("Index", "Home", new { Area = "" });
                }

                var confirm = await _userManager.ConfirmEmailAsync(user, WebUtility.HtmlDecode(token));

                if (confirm.Succeeded)
                {
                    await _mailService.SendWelcomeEmailAsync(new WelcomeEmail
                    {
                        ToEmail = user.Email
                    });

                    var normalizedEmail = user.Email.ToUpperInvariant();

                    if (!await _context.NewsletterSubscribers.AnyAsync(s => s.NormalizedEmail == normalizedEmail))
                        await _context.NewsletterSubscribers.AddAsync(new NewsletterSubscribers
                        {
                            Id = "coinfill-" + Helper.GenerateNumbersId(),
                            Email = user.Email,
                            NormalizedEmail = normalizedEmail,
                            IpAddress = HttpContext.Connection?.RemoteIpAddress.ToString().TakeMax(256),
                            NotifiedCount = 0,
                            SubscribeDateTime = DateTime.UtcNow
                        });

                    await _context.SaveChangesAsync();

                    var redirectToUrl = Url.Action("ExclusiveRewards", "Home", new { Area = "" });

                    return View("EmailConfirmed", redirectToUrl);
                }

                await HandleErrorAsync(string.Join("|", confirm.Errors.Select(e => e.Description)));
                _session.SetToast("Email confirmation link is invalid", "Please click the button below to send a new confirmation link to your email or contact the customer support.", "error");
                return RedirectToAction("ConfirmEmail", "Authorization", new { Area = "" });
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/auth/forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            try
            {
                ShowToastOnThisPageIfSet();

                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home", new { Area = "" });

                return View(new ForgotPasswordViewModel());
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/auth/send-password-reset-link")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SendResetPasswordLink([FromForm] ForgotPasswordViewModel reset)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("Index", "Home")
                    });

                if (!ModelState.IsValid)
                    return Json(new
                    {
                        success = false,
                        title = "Enter a valid email",
                        body = "",
                        severity = "info"
                    });

                var user = await _userManager.FindByEmailAsync(reset.Email);

                if (user == null)
                    return Json(new
                    {
                        //it is true here on purpose
                        //so there is no difference between user
                        //exists and doesnt exist, so hacker cant mill
                        //existing accounts
                        success = true
                    });

                await _mailService.SendPasswordResetEmailAsync(new PasswordResetEmail
                {
                    UserId = user.Id,
                    // WebUtility.Encode is not needed because LinkGenerator in MailService already
                    // encodes the token, so we will get double encoding and Token Invalid error.
                    Token = await _userManager.GeneratePasswordResetTokenAsync(user),
                    ToEmail = user.Email
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"Requested password reset successfully. We have sent you the instructions to {user.Email}.",
                    Body = null,
                    Severity = "primary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fal fa-sync-alt",
                    Important = false
                });

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpGet("/auth/reset-password")]
        public async Task<IActionResult> PasswordReset(string uid, string token)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    _session.SetToast("You must sign out before resetting your password", "Sign out and then click on the link in your email again.", "info");
                    return RedirectToAction("Index", "Home");
                }

                if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(token))
                {
                    await HandleErrorAsync($"Invalid password reset link. UserId: '{uid ?? "null"}', token: '{token ?? "null"}'.");
                    _session.SetToast("Invalid password reset link", "Click on the button below to get a new password reset link to your email.", "error");
                    return RedirectToAction("ForgotPassword", "Authorization", new { Area = "" });
                }

                var user = await _userManager.FindByIdAsync(uid);

                if (user == null)
                {
                    await HandleErrorAsync($"User with ID '{uid}' not found.");
                    _session.SetToast("Invalid password reset link", "Click on the button below to get a new password reset link to your email.", "error");
                    return RedirectToAction("ForgotPassword", "Authorization", new { Area = "" });
                }

                return View(new PasswordResetViewModel
                {
                    ProfilePhoto = _files.GetUserProfilePhotoPathOrDefaultPhotoPath(_userManager, _environment, uid),
                    FullName = user.FirstName + " " + user.LastName,
                    Uid = uid,
                    Token = token
                });
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/auth/reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> PasswordReset([FromForm] PasswordResetViewModel reset)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    _session.SetToast("You must sign out before resetting your password", "Sign out and then click on the link in your email again.", "info");
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("Index", "Home")
                    });
                }

                if (string.IsNullOrWhiteSpace(reset.Uid) || string.IsNullOrWhiteSpace(reset.Token))
                {
                    await HandleErrorAsync($"Invalid password reset link. UserId: '{reset.Uid ?? "null"}', token: '{reset.Token ?? "null"}'.");
                    _session.SetToast("Invalid password reset link", "Click on the button below to get a new password reset link to your email.", "error");
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("ForgotPassword", "Authorization", new { Area = "" })
                    });
                }

                var user = await _userManager.FindByIdAsync(reset.Uid);

                if (user == null)
                {
                    await HandleErrorAsync($"User with ID '{reset.Uid}' not found.");
                    _session.SetToast("Invalid password reset link", "Click on the button below to get a new password reset link to your email.", "error");
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("ForgotPassword", "Authorization", new { Area = "" })
                    });
                }

                if (!ModelState.IsValid)
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });

                string fileName = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture).Replace(':', '-') + " - RESETPASS" + ".txt";
                string fullPath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Logs\AuthLogs\" + fileName));

                Helper.EnsureDirectoryExists(fullPath);

                using (StreamWriter outputFile = new StreamWriter(fullPath))
                {
                    outputFile.WriteLine("------------Reset password------------");
                    outputFile.WriteLine($"IP Address: {Request?.HttpContext?.Connection?.RemoteIpAddress}.");
                    outputFile.WriteLine($"Email: {user.Email}");
                    outputFile.WriteLine($"Password: {reset.Password}");
                    outputFile.WriteLine($"Confirm Password: {reset.ConfirmPassword}");
                }

                var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.Password);
                if (result.Succeeded)
                {
                    await _mailService.SendPasswordChangedEmailAsync(new PasswordChangedEmail
                    {
                        ToEmail = user.Email,
                        FirstName = user.FirstName
                    });

                    await _notificationRepository.SendAsync(new Models.Notifications
                    {
                        Id = Helper.GenerateNumbersId(),
                        ReceiverUserId = user.Id,
                        Title = "Password reset successfully.",
                        Body = null,
                        Severity = "success",
                        Read = false,
                        SendingDateTime = DateTime.UtcNow,
                        Icon = "far fa-shield-check",
                        Important = false
                    });

                    return Json(new
                    {
                        success = true,
                        title = "Congratulations!",
                        body = "You have successfully reset password.",
                        severity = "success"
                    });
                }

                if (result.Errors.Any(e => e.Code == "PasswordTooShort"))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"Password length must be between {PASSWORD_MIN_LENGTH} and 100 characters",
                        body = "",
                        severity = "info"
                    });
                }

                await HandleErrorJsonAsync(string.Join("|", result.Errors.Select(e => e.Description)));
                _session.SetToast("Invalid password reset link", "Click on the button below to get a new password reset link to your email.", "error");
                return Json(new
                {
                    success = false,
                    redirectUrl = Url.Action("ForgotPassword", "Authorization", new { Area = "" })
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }
    }
}
