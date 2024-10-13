using CoinFill.Emails;
using CoinFill.Helpers.Models;
using CoinFill.Helpers.Providers;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using CoinFill.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinFill.Helpers;
using CoinFill.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Threading;

namespace CoinFill.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppEmails _appEmails;

        public HomeController(IWebHostEnvironment environment,
            IConfiguration configuration,
            IErrorLogger error,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager,
            SignInManager<CustomClient> signInManager) : base(error, mailService, contextAccessor, notificationRepository, userManager, signInManager)
        {
            _environment = environment;
            _appEmails = configuration.GetSection("AppEmails").Get<AppEmails>();
        }

        [HttpGet("/hello")]
        public async Task<IActionResult> hello()
        {
            try
            {
                string fullPath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\domain.txt"));

                string sames = "";

                string data = System.IO.File.ReadAllText(fullPath);
                Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                    RegexOptions.IgnoreCase);
                MatchCollection emailMatches = emailRegex.Matches(data);
                foreach (Match emailMatch in emailMatches)
                {
                    try
                    {
                        var mm = await _context.NewsletterSubscribers.SingleOrDefaultAsync(xxx => xxx.Email == emailMatch.Value);
                        
                        if (mm != default)
                        {
                            mm.Verified = true;
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch(Exception eee)
                    {
                        await HandleErrorAsync("Error on email: " + emailMatch.Value);
                    }
                }

                return Content("All verified: " + emailMatches.Count);
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/send-all")]
        public async Task<IActionResult> SendEmails()
        {
            try
            {
                Random rnd = new Random();
                string all = "";
                foreach (var sub in await _context.NewsletterSubscribers.Where(s => s.NotifiedCount == 0 && s.VerificationCount == 3).Take(40).ToListAsync())
                {
                    Thread.Sleep(7000 + (rnd.Next(1, 13) * 1000));
                    try
                    {
                        await _mailService.SendNow(sub.Email);
                        sub.NotifiedCount = sub.NotifiedCount + 1;
                        sub.LastNotifiedDateTime = DateTime.UtcNow;
                        all = all + sub.Email + " ---\n";
                        _context.NewsletterSubscribers.Update(sub);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        await HandleErrorAsync($"This email cause error: {sub.Email} --- {ex.GetRootException().Message}");
                    }
                }

                return Content("these emails are notified: " + all);
            }
            catch(Exception e)
            {
                await HandleErrorAsync(e);
                return Content("Error: " + e.GetRootException().Message);
            }
        }

        public async Task<IActionResult> Index(bool subscribe = false, bool unsubscribe = false)
        {
            if (unsubscribe)
                _session.SetToast("Email successfully unsubscribed", "", "success");

            ShowToastOnThisPageIfSet();

            if (subscribe)
                ViewBag.Subscribe = true;

            return View(new IndexViewModel
            {
                SupportedCountriesCount = SupportedCountriesProvider.SupportedCountries.Count,
                SupportedCurrenciesCount = SupportedCurrenciesProvider.SupportedCurrencies.Count,
                SupportedCryptocurrencies = SupportedCryptocurrenciesProvider.SupportedCryptocurrencies.Count
            });
        }

        //[HttpGet("/airdrop")]
        //public IActionResult Airdrop()
        //{
        //    return View();
        //}

        [HttpGet("/about")]
        public IActionResult AboutUs()
        {
            ShowToastOnThisPageIfSet();
            return View();
        }

        [HttpGet("/contact")]
        public IActionResult ContactUs()
        {
            ShowToastOnThisPageIfSet();
            return View();
        }

        [HttpGet("/privacy")]
        public IActionResult Privacy()
        {
            ShowToastOnThisPageIfSet();
            return View();
        }

        [HttpGet("/terms")]
        public IActionResult Terms()
        {
            ShowToastOnThisPageIfSet();
            return View();
        }

        [HttpGet("/contact/support/{topic?}")]
        public async Task<IActionResult> CustomerSupport(string topic = null)
        {
            try
            {
                ShowToastOnThisPageIfSet();

                var isSignedIn = User.Identity.IsAuthenticated;
                var user = isSignedIn ? await _userManager.GetUserAsync(User) : null;

                var topics = new List<ClientSupportTicketTopics>
                {
                    CustomerSupportTicketTopicsProvider.Sales,
                    CustomerSupportTicketTopicsProvider.Support,
                    CustomerSupportTicketTopicsProvider.Marketing,
                    CustomerSupportTicketTopicsProvider.Payment,
                    CustomerSupportTicketTopicsProvider.ReportError,
                    CustomerSupportTicketTopicsProvider.AskQuestion,
                    CustomerSupportTicketTopicsProvider.GiveFeedback
                };

                if (!string.IsNullOrWhiteSpace(topic)) topic = topic.ToLower();

                if (isSignedIn)
                    topics.Add(CustomerSupportTicketTopicsProvider.RequestAccountDeletion);
                else if (!string.IsNullOrWhiteSpace(topic) && topic == CustomerSupportTicketTopicsProvider.RequestAccountDeletion.Id)
                    topic = null;

                var supportVm = new CustomerSupportViewModel
                {
                    Email = isSignedIn ? user.Email : null
                };

                foreach (var topicItem in topics)
                {
                    supportVm.ToChooseFrom_Topics.Add(new SelectListItem
                    {
                        Value = topicItem.Id,
                        Text = topicItem.Icon + "|" + topicItem.Name + "|" + topicItem.Color,
                        Selected = topicItem.Id == topic
                    });
                }

                return View(supportVm);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/contact/support")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CustomerSupport([FromForm] CustomerSupportViewModel support)
        {
            try
            {
                var isSignedIn = User.Identity.IsAuthenticated;
                var user = isSignedIn ? await _userManager.GetUserAsync(User) : null;

                if (!ModelState.IsValid)
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });

                var topics = new List<ClientSupportTicketTopics>
                {
                    CustomerSupportTicketTopicsProvider.Sales,
                    CustomerSupportTicketTopicsProvider.Support,
                    CustomerSupportTicketTopicsProvider.Marketing,
                    CustomerSupportTicketTopicsProvider.Payment,
                    CustomerSupportTicketTopicsProvider.ReportError,
                    CustomerSupportTicketTopicsProvider.AskQuestion,
                    CustomerSupportTicketTopicsProvider.GiveFeedback
                };

                var ticket = topics.Find(t => t.Id == support.Chosen_TopicId);

                if (!topics.Any(t => t.Id == support.Chosen_TopicId))
                {
                    _session.SetToast("The chosen topic is not valid", "Please choose another one", "info");

                    return Json(new
                    {
                        success = false,
                        redirectUrl = Url.Action("CustomerSupport", "Home", new { Area = "" })
                    });
                }

                if (isSignedIn)
                    await _notificationRepository.SendAsync(new Models.Notifications
                    {
                        Id = Helper.GenerateNumbersId(),
                        ReceiverUserId = user.Id,
                        Title = "Your query has been successfully sent to our support team.",
                        Body = null,
                        Severity = "primary",
                        Read = false,
                        SendingDateTime = DateTime.UtcNow,
                        Icon = "fal fa-comments",
                        Important = false
                    });

                await _mailService.SendSupportTicketAsync(new Emails.EmailTypes.WelcomeEmail
                {
                    ToEmail = support.Email.ToLower()
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW SUPPORT TICKET] {ticket.Name}",
                    Body = $"User with email '{support.Email}' has sent a ticket at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now}). Message: {support.Text}."
                });

                return Json(new
                {
                    success = true,
                    title = "Your query has been successfully sent to our support team.",
                    body = "Our representative will contact you via email soon.",
                    severity = "success",
                    redirectUrl = Url.Action("Index", "Home", new { Area = "" })
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/subscribe-to-newsletter")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> NewsletterSubscription(NewsletterSubscriptionViewModel subscribe)
        {
            try
            {

                if (!ModelState.IsValid)
                    return Json(new
                    {
                        success = false,
                        title = "Enter a valid email",
                        body = "",
                        severity = "info"
                    });

                if (await _context.NewsletterSubscribers.AsNoTracking().AnyAsync(l => l.NormalizedEmail == subscribe.Email.ToUpper()))
                    return Json(new
                    {
                        success = false,
                        title = "This email is already subscribed to our newsletter",
                        body = "",
                        severity = "info"
                    });

                await _context.NewsletterSubscribers.AddAsync(new NewsletterSubscribers
                {
                    Id = "coinfill-" + Helper.GenerateNumbersId(),
                    Email = subscribe.Email,
                    NormalizedEmail = subscribe.Email.ToUpper(),
                    NotifiedCount = 0,
                    IpAddress = HttpContext.Connection?.RemoteIpAddress.ToString().TakeMax(256),
                    SubscribeDateTime = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                await _mailService.SendNewNewsletterSubscriptionEmailAsync(new Emails.EmailTypes.WelcomeEmail
                {
                    ToEmail = subscribe.Email
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = "[NEW NEWSLETTER SUBSCRIBER]",
                    Body = $"User with email '{subscribe.Email}' has subscribed to newsletter at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                return Json(new
                {
                    success = true,
                    title = "Thank you!",
                    body = "You have successfully subscribed to our newsletter.",
                    severity = "success"
                });
            }
            catch (Exception e)
            {
                await HandleErrorJsonAsync(e);
                return Json(new
                {
                    success = false,
                    title = "An error occurred",
                    body = "Please refresh the page and try again or contact the customer support.",
                    severity = "error"
                });
            }
        }

        [HttpGet("/exclusive-rewards")]
        public async Task<IActionResult> ExclusiveRewards()
        {
            try
            {
                ShowToastOnThisPageIfSet();
                return View();
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/prcko/nest")]
        public async Task<IActionResult> Insert()
        {
            try
            {
                string fullPath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, @"..\CoinFill\Emails\yahoo.txt"));

                string sames = "";

                string data = System.IO.File.ReadAllText(fullPath);
                Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                    RegexOptions.IgnoreCase);
                MatchCollection emailMatches = emailRegex.Matches(data);

                foreach(var emailInDatabase in _context.NewsletterSubscribers.AsNoTracking().Where(dd => dd.Email.Contains("yahoo")).Select(xx => xx.Email))
                {
                    foreach(Match emailInFile in emailMatches)
                    {
                        if (emailInDatabase == emailInFile.Value) sames = sames + " ----\n ";
                    }
                }

                return Content("Sve ok: " + sames);
            }
            catch (Exception e)
            {
                await HandleErrorAsync(e);
                return Content("Error: " + e.GetRootException().Message);
            }
        }
    }
}
