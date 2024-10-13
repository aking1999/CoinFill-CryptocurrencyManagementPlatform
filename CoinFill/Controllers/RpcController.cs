using CoinFill.Emails;
using CoinFill.Helpers.Extensions;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using CoinFill.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace CoinFill.Controllers
{
    public class RpcController : BaseController
    {
        private readonly IWebHostEnvironment _environment;

        public RpcController(IWebHostEnvironment environment,
            IErrorLogger error,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager,
            SignInManager<CustomClient> signInManager) : base(error, mailService, contextAccessor, notificationRepository, userManager, signInManager)
        {
            _environment = environment;
        }

        [HttpGet("/{password}/rpc/cards/accept/{cardId}")]
        public async Task<IActionResult> AcceptCard(string password, string cardId)
        {
            try
            {
                if (password == "123")
                {
                    if (!string.IsNullOrWhiteSpace(cardId))
                    {
                        cardId = cardId.ToLower();
                        var card = await _context.UserCards.SingleOrDefaultAsync(c => c.Id == cardId);

                        if (card == default)
                            return Content("Not found");

                        if (card.ActivationStatus == 1)
                            return Content("Already accepted");

                        card.ActivationStatus = 1;

                        var user = await _userManager.FindByIdAsync(card.UserId);

                        if (user == default)
                            return Content($"User with card '{card.Id}' no longer exists, therefore the card '{card.Id}' can be deleted from DB.");

                        var crypto = await _context.Cryptocurrencies.SingleOrDefaultAsync(c => c.Id == card.PaymentMethodCryptocurrencyId);

                        await _mailService.SendCardAcceptedAsync(new Emails.EmailTypes.CardAcceptedEmail
                        {
                            CardId = card.Id,
                            CardBrand = card.Brand,
                            CardType = card.Type.FirstCharToUpper(),
                            ToEmail = user.Email,
                            CoinNameUsedForPayment = crypto.Name,
                            CardNumberEndingDigits = card.Number.Replace("*", string.Empty).Replace(" ", string.Empty)
                        });

                        _context.UserCards.Update(card);
                        await _context.SaveChangesAsync();

                        return Content($"{card.Type} {card.Brand} card '{card.Id}' accepted.");
                    }
                    return Content("cardId is null");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                await HandleErrorAsync(e);
                return Content("Error");
            }
        }

        [HttpGet("/{password}/rpc/email")]
        public async Task<IActionResult> Email(string password)
        {
            try
            {
                if (password == "I_have_the_best_MOM_ever!!!")
                {
                    return View(new RpcEmail());
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                await HandleErrorAsync(e);
                return Content("Error");
            }
        }

        [HttpPost("/{password}/rpc/email")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Email([FromForm]RpcEmail email)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                await _mailService.SendCaptureTransactionSuccessfulAsync(new Emails.EmailTypes.CaptureTransactionEmail
                {
                    ToEmail = email.ToEmail,
                    Subject = email.Subject,
                    Header = email.InnerEmailHeader,
                    Body = email.Body
                });

                return Json(new
                {
                    success = true,
                    title = "Email send successfully",
                    body = $"Email send to {email.ToEmail} successfully.",
                    severity = "success"
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpGet("/stop")]
        public IActionResult Stop()
        {
            SoundPlayer sound = new SoundPlayer(Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\notify.wav")));
            sound.Stop();
            return RedirectToAction("Index", "Home");
        }
    }
}