using CoinFill.Emails;
using CoinFill.Helpers.Models;
using CoinFill.Helpers.Providers;
using CoinFill.Implementations;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using CoinFill.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Controllers
{
    [Authorize(Roles = "Client")]
    public class AccountController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ICustomClientFunctionsProvider _clientFunctions;

        public AccountController(IWebHostEnvironment environment,
            IErrorLogger errors,
            IMailService mailService,
            //IDateTimeHelper dateHelper,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager) : base(errors, mailService, contextAccessor, notificationRepository, userManager)
        {
            _environment = environment;
            _clientFunctions = new CustomClientFunctionsProvider(contextAccessor);
        }

        [HttpGet("/dashboard/account")]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var emailStatus = await _clientFunctions.HasConfirmedEmailAsync();
                if (emailStatus == EmailConfirmationStatus.Error)
                    throw new GeneralException("unable to load user", signOutUser: true);
                else if (emailStatus == EmailConfirmationStatus.NotConfirmed)
                    return RedirectToAction("ConfirmEmail", "Authorization", new { Area = "" });

                ShowToastOnThisPageIfSet();

                var user = await _userManager.GetUserAsync(User);

                var profileVm = new UserProfileViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    HasCompletedAddress = !string.IsNullOrWhiteSpace(user.FullNameAddress) && !string.IsNullOrWhiteSpace(user.Street) &&
                                          !string.IsNullOrWhiteSpace(user.HouseNumber) && !string.IsNullOrWhiteSpace(user.City) &&
                                          !string.IsNullOrWhiteSpace(user.PostalCode) && !string.IsNullOrWhiteSpace(user.Country)
                };

                return View(profileVm);
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/dashboard/account/edit-profile")]
        public async Task<IActionResult> EditProfile(IFormFile photo)
        {
            try
            {
                var emailStatus = await _clientFunctions.HasConfirmedEmailAsync();
                if (emailStatus == EmailConfirmationStatus.Error)
                    throw new GeneralException("unable to load user", signOutUser: true);
                else if (emailStatus == EmailConfirmationStatus.NotConfirmed)
                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("ConfirmEmail", "Authorization", new { Area = "" })
                    });

                var user = await _userManager.GetUserAsync(User);

                if (photo == null)
                {
                    await HandleErrorAsync($"User '{user.Id}' is missing photo from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (string.IsNullOrWhiteSpace(photo.FileName))
                {
                    await HandleErrorAsync($"User '{user.Id}' is missing photo.FileName from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                IFileRepository _files = new FileRepository();

                _files.DeleteUserImage(_environment, user.ProfilePhoto);
                user.ProfilePhoto = await _files.CreateUserImageAsync(_environment, $"client-{user.FirstName}-{user.Id}", photo);

                var update = await _userManager.UpdateAsync(user);
                if (!update.Succeeded) throw new GeneralException(string.Join("|", update.Errors.Select(e => e.Description)));

                return Json(new
                {
                    success = true,
                    title = "Profile photo update successfully",
                    body = "",
                    severity = "success"
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpGet("/dashboard/account/edit-address")]
        public async Task<IActionResult> EditAddress()
        {
            try
            {
                var emailStatus = await _clientFunctions.HasConfirmedEmailAsync();
                if (emailStatus == EmailConfirmationStatus.Error)
                    throw new GeneralException("unable to load user", signOutUser: true);
                else if (emailStatus == EmailConfirmationStatus.NotConfirmed)
                    return RedirectToAction("ConfirmEmail", "Authorization", new { Area = "" });

                ShowToastOnThisPageIfSet();

                var user = await _userManager.GetUserAsync(User);

                return View(new EditAddressViewModel
                {
                    FullName = user.FullNameAddress,
                    Street = user.Street,
                    HouseNumber = user.HouseNumber,
                    City = user.City,
                    PostalCode = user.PostalCode,
                    Chosen_CountryId = user.Country,
                    ToChooseFrom_Countries = _clientFunctions.GetSupportedCountries()
                });
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/dashboard/account/edit-address")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditAddress([FromForm]EditAddressViewModel addressVm)
        {
            try
            {
                try
                {
                    var emailStatus = await _clientFunctions.HasConfirmedEmailAsync();
                    if (emailStatus == EmailConfirmationStatus.Error)
                        throw new GeneralException("unable to load user", signOutUser: true);
                    else if (emailStatus == EmailConfirmationStatus.NotConfirmed)
                        return Json(new
                        {
                            success = false,
                            redirectUrl = Url.Action("ConfirmEmail", "Authorization", new { Area = "" })
                        });

                    var user = await _userManager.GetUserAsync(User);

                    if (!ModelState.IsValid)
                    {
                        await HandleErrorJsonAsync($"User '{user.Id}' entered invalid ModelState data.");
                        return Json(new
                        {
                            success = false,
                            title = "Fill all the required fields",
                            body = "",
                            severity = "info"
                        });
                    }

                    if(!SupportedCountriesProvider.SupportedCountries.Any(c => c.Name == addressVm.Chosen_CountryId))
                    {
                        await HandleErrorJsonAsync($"User '{user.Id}' chose an unexistent country.");
                        return Json(new
                        {
                            success = false,
                            title = "Selected country does not exist",
                            body = "Please refresh the page and try again.",
                            severity = "info"
                        });
                    }

                    user.FullNameAddress = addressVm.FullName;
                    user.Street = addressVm.Street;
                    user.HouseNumber = addressVm.HouseNumber;
                    user.City = addressVm.City;
                    user.PostalCode = addressVm.PostalCode;
                    user.Country = addressVm.Chosen_CountryId;

                    var update = await _userManager.UpdateAsync(user);

                    if (!update.Succeeded) throw new Exception($"User '{user.Id}' could not update his address data. {string.Join('|', update.Errors.Select(e => e.Description))}");

                    return Json(new
                    {
                        success = true,
                        title = "You have successfully updated your address",
                        body = "Now you can order your physical Crypto card.",
                        severity = "success",
                        orderCryptoCardUrl = Url.Action("Cards", "Dashboard", new { Area = "" })
                    });
                }
                catch (Exception e)
                {
                    return await HandleErrorJsonAsync(e);
                }
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }
    }
}
