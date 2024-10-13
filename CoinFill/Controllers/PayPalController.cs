using CoinFill.Emails;
using CoinFill.Helpers;
using CoinFill.Helpers.Models;
using CoinFill.Implementations;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using CoinFill.PartialViewModels;
using CoinFill.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Controllers
{
    [Authorize(Roles = "Client")]
    public class PayPalController : BaseController
    {
        private readonly AppEmails _appEmails;
        private readonly IWebHostEnvironment _environment;
        private readonly ICustomClientFunctionsProvider _clientFunctions;

        public PayPalController(IConfiguration configuration,
            IWebHostEnvironment environment,
            IErrorLogger errors,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager) : base(errors, mailService, contextAccessor, notificationRepository, userManager)
        {
            _appEmails = configuration.GetSection("AppEmails").Get<AppEmails>();
            _environment = environment;
            _clientFunctions = new CustomClientFunctionsProvider(contextAccessor);
        }

        [HttpGet("/dashboard/paypal/transfer")]
        public async Task<IActionResult> TransferToPayPal()
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

                return View(new PayPalViewModel());
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/dashboard/paypal/get-paypal-withdraw-page")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetPayPalWithdrawCapturePage([FromForm] PayPalViewModel paypalVm)
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

                ShowToastOnThisPageIfSet();

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

                string fee = "0.25";

                if (paypalVm.PayPalTransferType.ToLower() == "goods and services")
                    fee = "2.99";

                return Json(new
                {
                    success = true,
                    partialView = await RenderPartialViewToStringAsync("_CapturePayPalDeposit", new CaptureTransaction
                    {
                        Id = Helper.GenerateNumbersId(),
                        AlertMessage = $"The cryptocurrency amount you send will be converted to USD and sent to your PayPal account ({fee}% fee)." +
                        $"<br><br>" +
                        $"It takes 30 minutes to verify the deposit, do the conversion and send the amount to your PayPal account.",
                        HeaderText = "Choose your preferred cryptocurrency for PayPal deposit",
                        DropdownPlaceholderText = "Preferred cryptocurrency",
                        ChosenTransactionIdLabel = $"Choose the cryptocurrency with which you would like to make a deposit into your PayPal account",
                        IsBank = true,
                        ToChooseFrom_TransactionMethods = await _clientFunctions.GetSupportedCryptocurrenciesAsync()
                    })
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/paypal/transfer")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> PayPalDeposit(string paypalRequestId, string coinId, string networkId, string email)
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

                if (string.IsNullOrWhiteSpace(paypalRequestId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing paypalRequestId from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (string.IsNullOrWhiteSpace(coinId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing coinId from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (string.IsNullOrWhiteSpace(networkId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing networkId from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing email from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "PayPal email is required",
                        body = "Please enter the email linked to the PayPal account where you would like to send the money.",
                        severity = "info"
                    });
                }

                paypalRequestId = paypalRequestId.ToLower();
                coinId = coinId.ToLower();
                networkId = networkId.ToLower();
                email = email.ToLower();

                var crypto = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == coinId);

                await _context.Payments.AddAsync(new Payments
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    Reason = "paypal-deposit",
                    ReasonId = email,
                    AmountShouldBe = (decimal)crypto.MinimumTransactionAmount,
                    PaymentMethodCryptocurrencyId = coinId,
                    PaymentMethodCryptocurrencyNetworkId = networkId,
                    ActivationStatus = 0,
                    CreatedDateTime = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully requested {crypto.Name} deposit into your PayPal account.",
                    Body = "",
                    Severity = "primary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fab fa-paypal",
                    Important = false
                });

                await _mailService.SendCaptureTransactionSuccessfulAsync(new Emails.EmailTypes.CaptureTransactionEmail
                {
                    ToEmail = user.Email,
                    Subject = $"PayPal deposit requested successfully",
                    Header = $"You have successfully requested {crypto.Name} deposit into your PayPal account",
                    Body = $"Please be patient until we verify your {crypto.Name} payment. Once verified, the deposit will be sent to your PayPal account and " +
                    $"will be settled in USD."
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW PAYPAL DEPOSIT] PayPal account deposit request",
                    Body = $"User '{user.Id}' requested a {crypto.Name} deposit to PayPal account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                return Json(new
                {
                    success = true,
                    title = "Your transaction will be verified in 30 minutes",
                    body = $"Once your deposit is verified, the amount will be sent to your PayPal account and you will get a confirmation email at {user.Email}. " +
                    $"It takes 1-3 business day(s) for the deposit to reach your PayPal account. Upon arrival, the amount will be converted to USD and settled in your PayPal account.",
                    severity = "success"
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }
    }
}
