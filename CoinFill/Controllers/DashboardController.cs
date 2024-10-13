using CoinFill.Emails;
using CoinFill.Helpers;
using CoinFill.Helpers.Extensions;
using CoinFill.Helpers.Models;
using CoinFill.Implementations;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using CoinFill.PartialViewModels;
using CoinFill.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using CoinFill.Helpers.Providers;

namespace CoinFill.Controllers
{
    [Authorize(Roles = "Client")]
    public class DashboardController : BaseController
    {
        private readonly AppEmails _appEmails;
        private readonly CryptocurrencyApi _cryptocurrencyApi;
        private readonly ICustomClientFunctionsProvider _clientFunctions;

        public DashboardController(IConfiguration configuration,
            IErrorLogger errors,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager) : base(errors, mailService, contextAccessor, notificationRepository, userManager)
        {
            _appEmails = configuration.GetSection("AppEmails").Get<AppEmails>();
            _cryptocurrencyApi = configuration.GetSection("CryptocurrencyApi").Get<CryptocurrencyApi>();
            _clientFunctions = new CustomClientFunctionsProvider(contextAccessor);
        }

        public async Task<IActionResult> Index()
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

                var dashboard = new DashboardViewModel();

                string brand = string.Empty;
                CardBrandDimensions brandDimensions = null;

                // Adding Crypto Cards
                foreach (var card in (await _context.UserCards.AsNoTracking().Where(c => c.UserId == user.Id && (c.ActivationStatus == -1 || c.ActivationStatus == 0 || c.ActivationStatus == 1)).ToListAsync()).OrderBy(c => c.RequestedDateTime))
                {
                    brand = string.Concat(card.Brand.Where(c => !char.IsWhiteSpace(c))).ToLowerInvariant();
                    brandDimensions = _clientFunctions.GetBrandDimensions(brand);

                    dashboard.Cards.Add(new CardViewModel
                    {
                        Id = card.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Type = card.Type,
                        Brand = brand + ".png",
                        Width = brandDimensions.Width,
                        Height = brandDimensions.Height,
                        BootstrapClasses = brandDimensions.BootstrapClasses,
                        Number = card.Number,
                        ExpirationDate = card.ExpirationDate,
                        Cvv = card.Cvv,
                        BackgroundImage = card.BackgroundImage,
                        Balance = card.Balance.ToString("0.##"),
                        ActivationStatus = card.ActivationStatus
                    });
                }

                // Adding Banks Accounts
                foreach (var bankAccount in _context.UserBankAccounts.AsNoTracking().Where(b => b.UserId == user.Id).OrderBy(b => b.AddedDateTime))
                {
                    if (string.IsNullOrWhiteSpace(bankAccount.BicSwift))
                        bankAccount.BicSwift = "N/A - not required";

                    if (bankAccount.Currency == "EUR")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.EUR,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "EUR",
                            BankAccountNumber = bankAccount.BankAccountNumber,
                            BicSwift = bankAccount.BicSwift
                        });
                    }
                    else if (bankAccount.Currency == "USD")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.USD,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "USD",
                            BankAccountNumber = bankAccount.BankAccountNumber,
                            RoutingNumber = bankAccount.RoutingNumber
                        });
                    }
                    else if (bankAccount.Currency == "GBP")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.GBP,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "GBP",
                            BankAccountNumber = bankAccount.BankAccountNumber,
                            BicSwift = bankAccount.BicSwift
                        });
                    }
                    else if (bankAccount.Currency == "CHF")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.CHF,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "CHF",
                            BankAccountNumber = bankAccount.BankAccountNumber,
                            BicSwift = bankAccount.BicSwift
                        });
                    }
                    else if (bankAccount.Currency == "CNY")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Card",
                            Id = bankAccount.Id,
                            IdLabel = "Card",
                            Fee = BankAccountDepositFeeProvider.CNY,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "CNY",
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "HKD")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.HKD,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "HKD",
                            BankAccountNumber = bankAccount.BankAccountNumber,
                            BicSwift = bankAccount.BicSwift
                        });
                    }
                    else if (bankAccount.Currency == "RUB")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.RUB,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "RUB",
                            BankAccountNumber = bankAccount.BankAccountNumber,
                            BicSwift = bankAccount.BicSwift
                        });
                    }
                    else if (bankAccount.Currency == "IRR")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Card",
                            Id = bankAccount.Id,
                            IdLabel = "Card",
                            Fee = BankAccountDepositFeeProvider.IRR,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "IRR",
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "KRW")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.KRW,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "KRW",
                            BankAccountNumber = bankAccount.BankAccountNumber,
                            BicSwift = bankAccount.BicSwift
                        });
                    }
                    else if (bankAccount.Currency == "CAD")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.CAD,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "CAD",
                            TransitNumber = bankAccount.TransitNumber,
                            InstitutionNumber = bankAccount.InstitutionNumber,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "AUD")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.AUD,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "AUD",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "NOK")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.NOK,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "NOK",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "DKK")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.DKK,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "DKK",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "SEK")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.SEK,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "SEK",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "INR")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.INR,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "INR",
                            Ifsc = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "HUF")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.HUF,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "HUF",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "RSD")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.RSD,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "RSD",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "BAM")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.BAM,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "BAM",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "PLN")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.PLN,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "PLN",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "UAH")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.UAH,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "UAH",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "CZK")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.CZK,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "CZK",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "RON")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.RON,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "RON",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "BGN")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.BGN,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "BGN",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "TRY")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.TRY,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "TRY",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "AED")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.AED,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "AED",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "ILS")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.ILS,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "ILS",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "MXN")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.MXN,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "MXN",
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "PKR")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.PKR,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "PKR",
                            BicSwift = bankAccount.BicSwift,
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                    else if (bankAccount.Currency == "MAD")
                    {
                        dashboard.BankAccounts.Add(new BankAccountViewModel
                        {
                            AccountType = "Bank Account",
                            Id = bankAccount.Id,
                            Fee = BankAccountDepositFeeProvider.MAD,
                            FirstName = bankAccount.FirstName,
                            LastName = bankAccount.LastName,
                            Currency = "MAD",
                            BankAccountNumber = bankAccount.BankAccountNumber
                        });
                    }
                }

                return View(dashboard);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/dashboard/cards/{cardId?}")]
        public async Task<IActionResult> Cards(string cardId = null)
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

                var cardsPageVm = new CardsPageViewModel
                {
                    SupportedCountriesCount = SupportedCountriesProvider.SupportedCountries.Count,
                    Email = user.Email
                };

                string brand = string.Empty;
                CardBrandDimensions brandDimensions = null;

                foreach (var card in (await _context.UserCards.AsNoTracking().Where(c => c.UserId == user.Id && (c.ActivationStatus == -1 || c.ActivationStatus == 0 || c.ActivationStatus == 1)).ToListAsync()).OrderBy(c => c.RequestedDateTime))
                {
                    brand = string.Concat(card.Brand.Where(c => !char.IsWhiteSpace(c))).ToLowerInvariant();
                    brandDimensions = _clientFunctions.GetBrandDimensions(brand);

                    cardsPageVm.Cards.Add(new CardViewModel
                    {
                        Id = card.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Type = card.Type,
                        Brand = brand + ".png",
                        Width = brandDimensions.Width,
                        Height = brandDimensions.Height,
                        BootstrapClasses = brandDimensions.BootstrapClasses,
                        Number = card.Number,
                        ExpirationDate = card.ExpirationDate,
                        Cvv = card.Cvv,
                        BackgroundImage = card.BackgroundImage,
                        Balance = card.Balance.ToString("0.##"),
                        ActivationStatus = card.ActivationStatus
                    });
                }

                cardsPageVm.Address = string.Empty;

                if(!string.IsNullOrWhiteSpace(user.FullNameAddress) && !string.IsNullOrWhiteSpace(user.Street) &&
                   !string.IsNullOrWhiteSpace(user.HouseNumber) && !string.IsNullOrWhiteSpace(user.City) &&
                   !string.IsNullOrWhiteSpace(user.PostalCode) && !string.IsNullOrWhiteSpace(user.Country))
                {
                    cardsPageVm.Address = $"{user.FullNameAddress}<br>" +
                        $"{user.Street} {user.HouseNumber}<br>" +
                        $"{user.City} {user.PostalCode}<br>" +
                        $"{user.Country}";
                }

                cardsPageVm.ToChooseFrom_PaymentMethods = await _clientFunctions.GetSupportedCryptocurrenciesAsync();

                if (!string.IsNullOrWhiteSpace(cardId))
                {
                    cardsPageVm.CardIdToVerify = cardId;

                    if (cardId.ToLower() != "add-new")
                    {
                        var cardToVerify = cardsPageVm.Cards.SingleOrDefault(c => c.Id == cardId.ToLower());
                        if (cardToVerify != default)
                        {
                            if (cardToVerify.ActivationStatus == 1)
                            {
                                ViewBag.CardVerificationHeaderMessage = $"{cardToVerify.Type.FirstCharToUpper()} {cardToVerify.Brand.Split('.')[0].FirstCharToUpper()} card is verified";
                                ViewBag.CardVerificationBodyMessage = "You can top up the card balance and start using it right away.";
                                ViewBag.CardVerificationSeverity = "success";
                                cardsPageVm.CardWalletAlreadySubmitted = true;
                                cardsPageVm.StartTopTup = true;
                            }
                            else if (cardToVerify.ActivationStatus == 0)
                            {
                                ViewBag.CardVerificationHeaderMessage = $"Wallet address and payment are pending verification";
                                ViewBag.CardVerificationBodyMessage = "You have already submitted your wallet address for verification. Please be patient while the verification is completed.";
                                ViewBag.CardVerificationSeverity = "info";
                                cardsPageVm.CardWalletAlreadySubmitted = true;
                            }
                            else if (cardToVerify.ActivationStatus == -1)
                            {
                                ViewBag.CardVerificationHeaderMessage = $"Wallet address and payment are pending verification";
                                ViewBag.CardVerificationBodyMessage = "You have already submitted your wallet address for verification. Please be patient while the verification is completed.";
                                ViewBag.CardVerificationSeverity = "info";
                                cardsPageVm.CardWalletAlreadySubmitted = false;
                            }
                            else if (cardToVerify.ActivationStatus == -2)
                            {
                                ViewBag.CardVerificationHeaderMessage = $"Wallet address rejected";
                                ViewBag.CardVerificationBodyMessage = "Your wallet address has been rejected either due to an incorrect address or failure to complete the payment.";
                                ViewBag.CardVerificationSeverity = "error";
                                cardsPageVm.CardWalletAlreadySubmitted = true;
                            }
                        }
                        else
                        {
                            ViewBag.CardVerificationHeaderMessage = "Requested card not found";
                            ViewBag.CardVerificationBodyMessage = "The card you requested does not exist.";
                            ViewBag.CardVerificationSeverity = "info";
                            cardsPageVm.CardWalletAlreadySubmitted = true;
                        }
                    }
                    else
                    {
                        ViewBag.AddNew = true;
                    }
                }

                return View(cardsPageVm);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/dashboard/get-coin-networks")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetNetworks(string coinId)
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

                coinId = coinId.ToLower();
                var networks = _context.CryptocurrencyNetworks.AsNoTracking().Where(n => n.CryptocurrencyId == coinId);

                if (!await networks.AnyAsync())
                    return Json(new
                    {
                        success = false,
                        networks = networks
                    });

                var networksOrdered = await networks.OrderBy(n => n.OrderNumber).ToListAsync();

                return Json(new
                {
                    success = true,
                    networks = networksOrdered
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/get-user-qr-code")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetUserQrCode(string coinId, string networkId, string cardType, string cardBrand)
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

                coinId = coinId.ToLower();
                networkId = networkId.ToLower();

                if (string.IsNullOrWhiteSpace(cardType))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardType from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (string.IsNullOrWhiteSpace(cardBrand))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardBrand from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                decimal cardPriceUsd = 0;

                if (cardType == "virtual")
                    cardPriceUsd = 50;
                else if (cardType == "physical")
                    cardPriceUsd = 150;
                else
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent cardType '{cardType}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (!_clientFunctions.CardBrandExists(cardBrand))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent cardBrand '{cardBrand}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (!await _context.CryptocurrencyNetworks.AsNoTracking().AnyAsync(n => n.CryptocurrencyId == coinId))
                    return Json(new
                    {
                        success = false,
                        title = "No available blockchain for the selected cryptocurrency",
                        body = "",
                        severity = "info"
                    });

                GeneratedCryptocurrencyNetworks generated = null;

                try
                {
                    generated = await _context.GeneratedCryptocurrencyNetworks.AsNoTracking().SingleOrDefaultAsync(n => n.UserId == user.Id && n.CryptocurrencyId == coinId && n.CryptocurrencyNetworkId == networkId);
                }
                catch (Exception)
                {
                    generated = default;
                }

                var network = await _context.CryptocurrencyNetworks.AsNoTracking().SingleOrDefaultAsync(n => n.CryptocurrencyId == coinId && n.Id == networkId);

                if (network == default)
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent networkId '{networkId}' for cryptocurrencyId '{coinId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                var cryptocurrency = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == network.CryptocurrencyId);

                if (cryptocurrency == default)
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent cryptocurrencyId '{network.CryptocurrencyId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                var URL = new UriBuilder($"{_cryptocurrencyApi.BaseUrl}/v2/cryptocurrency/quotes/latest");

                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["id"] = coinId;

                URL.Query = queryString.ToString();

                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", _cryptocurrencyApi.ApiKey);
                client.Headers.Add("Accepts", "application/json");
                var returnedString = client.DownloadString(URL.ToString());

                var result = JsonConvert.DeserializeObject<JToken>(returnedString);
                if (result == null)
                    throw new Exception("Result is null.");

                if (result["data"] == null)
                    throw new Exception("JSON Object's 'data' property is null.");

                var cryptoPrice = decimal.Parse(result["data"][coinId]["quote"]["USD"]["price"].ToString());
                var cryptoAmountToPay = (cardPriceUsd / cryptoPrice).ToString("0.##");

                return Json(new
                {
                    success = true,
                    generated = generated != default,
                    notice = $"Please send {cryptoAmountToPay} {cryptocurrency.Name} (${cardPriceUsd - 1}, fees included) to this address, using the {network.Name} blockchain.",
                    qrCode = generated != default ? network.QrImage : "",
                    address = generated != default ? network.Address : "",
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/generate-network-address")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GenerateNetworkAddress(string coinId, string networkId, string cardType, string cardBrand)
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

                if (string.IsNullOrWhiteSpace(cardType))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardType from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (string.IsNullOrWhiteSpace(cardBrand))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardBrand from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                decimal cardPriceUsd = 0;

                if (cardType == "virtual")
                    cardPriceUsd = 50;
                else if (cardType == "physical")
                    cardPriceUsd = 150;
                else
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent cardType '{cardType}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (!_clientFunctions.CardBrandExists(cardBrand))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent cardBrand '{cardBrand}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (!await _context.CryptocurrencyNetworks.AsNoTracking().AnyAsync(n => n.CryptocurrencyId == coinId))
                    return Json(new
                    {
                        success = false,
                        title = "No available blockchain for the selected cryptocurrency",
                        body = "",
                        severity = "info"
                    });

                var network2 = await _context.CryptocurrencyNetworks.AsNoTracking().SingleOrDefaultAsync(n => n.CryptocurrencyId == coinId && n.Id == networkId);
                var cryptocurrency2 = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == network2.CryptocurrencyId);

                if (!await _context.GeneratedCryptocurrencyNetworks.AnyAsync(c => c.UserId == user.Id && c.CryptocurrencyId == cryptocurrency2.Id && c.CryptocurrencyNetworkId == network2.Id))
                {
                    await _context.GeneratedCryptocurrencyNetworks.AddAsync(new GeneratedCryptocurrencyNetworks
                    {
                        UserId = user.Id,
                        CryptocurrencyId = cryptocurrency2.Id,
                        CryptocurrencyNetworkId = network2.Id,
                        GenerationDateTime = DateTime.UtcNow
                    });

                    await _context.SaveChangesAsync();

                    await _notificationRepository.SendAsync(new Models.Notifications
                    {
                        Id = Helper.GenerateNumbersId(),
                        ReceiverUserId = user.Id,
                        Title = $"{cryptocurrency2.Name} ({network2.Name}) address generated successfully.",
                        Body = "",
                        Severity = "primary",
                        Read = false,
                        SendingDateTime = DateTime.UtcNow,
                        Icon = "fal fa-qrcode",
                        Important = false
                    });

                    await _mailService.SendNetworkAddressGeneratedAsync(new Emails.EmailTypes.NetworkAddressGeneratedEmail
                    {
                        ToEmail = user.Email,
                        CoinName = cryptocurrency2.Name,
                        NetworkName = network2.Name,
                        Address = network2.Address
                    });

                    await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                    {
                        ToEmail = _appEmails.NotificationEmail,
                        Subject = $"[NEW ADDRESS] {cryptocurrency2.Name} ({network2.Name}) address generated",
                        Body = $"User '{user.Id}' generated a {cryptocurrency2.Name} ({network2.Name}) address generated at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                    });
                }

                var URL = new UriBuilder($"{_cryptocurrencyApi.BaseUrl}/v2/cryptocurrency/quotes/latest");

                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["id"] = coinId;

                URL.Query = queryString.ToString();

                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", _cryptocurrencyApi.ApiKey);
                client.Headers.Add("Accepts", "application/json");
                var returnedString = client.DownloadString(URL.ToString());

                var result = JsonConvert.DeserializeObject<JToken>(returnedString);
                if (result == null)
                    throw new Exception("Result is null.");

                if (result["data"] == null)
                    throw new Exception("JSON Object's 'data' property is null.");

                var cryptoPrice = decimal.Parse(result["data"][coinId]["quote"]["USD"]["price"].ToString());
                var cryptoAmountToPay = (cardPriceUsd / cryptoPrice).ToString("0.##");

                return Json(new
                {
                    success = true,
                    title = $"{cryptocurrency2.Name} ({network2.Name}) address generated successfully.",
                    body = "",
                    severity = "success",
                    notice = $"Please send {cryptoAmountToPay} {cryptocurrency2.Name} (${cardPriceUsd - 1}, fees included) to this address, using the {network2.Name} blockchain.",
                    cryptocurrencyName = cryptocurrency2.Name,
                    networkName = network2.Name,
                    qrCode = network2.QrImage,
                    address = network2.Address
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/get-sender-address")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetSenderAddressCapturePage(string coinId, string networkId, string cardType, string cardBrand)
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

                if (string.IsNullOrWhiteSpace(cardType))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardType from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (string.IsNullOrWhiteSpace(cardBrand))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardBrand from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (!_clientFunctions.CardTypeExists(cardType))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent cardType '{cardType}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (!_clientFunctions.CardBrandExists(cardBrand))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent cardBrand '{cardBrand}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if(cardType.ToLowerInvariant() == "physical" && (string.IsNullOrWhiteSpace(user.FullNameAddress) || string.IsNullOrWhiteSpace(user.Street) ||
                   string.IsNullOrWhiteSpace(user.HouseNumber) || string.IsNullOrWhiteSpace(user.City) ||
                   string.IsNullOrWhiteSpace(user.PostalCode) || string.IsNullOrWhiteSpace(user.Country)))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has not added his address but tried to order a physical card.");
                    return Json(new
                    {
                        success = false,
                        title = "Shipping address not added",
                        body = "You can get the physical Crypto card only after adding your address.",
                        severity = "info",
                        addAddressUrl = Url.Action("EditAddress", "Account", new { Area = "" })
                    });
                }

                var userCard = new UserCards
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    Type = cardType.TakeMax(127),
                    Brand = cardBrand.TakeMax(127),
                    Number = $"**** **** **** {Helper.GenerateNumbersId().TakeMax(4)}",
                    ExpirationDate = "**/**",
                    Cvv = "***",
                    Balance = 0,
                    BackgroundImage = new Random().Next(0, 10) + ".png",
                    PaymentMethodCryptocurrencyId = coinId,
                    PaymentMethodCryptocurrencyNetworkId = networkId,
                    ActivationStatus = -1,
                    RequestedDateTime = DateTime.UtcNow
                };

                if(cardType.ToLowerInvariant() == "physical")
                {
                    userCard.FullNameAddress = user.FullNameAddress;
                    userCard.Street = user.Street;
                    userCard.HouseNumber = user.HouseNumber;
                    userCard.City = user.City;
                    userCard.PostalCode = user.PostalCode;
                    userCard.Country = user.Country;
                }

                if (cardBrand.ToLowerInvariant() == "american express")
                    userCard.Number = $"**** ****** *{Helper.GenerateNumbersId().TakeMax(4)}";

                await _context.UserCards.AddAsync(userCard);
                await _context.SaveChangesAsync();

                var paymentCryptocurrencyName = (await _context.Cryptocurrencies.SingleOrDefaultAsync(c => c.Id == coinId))?.Name ?? "";

                var brand = string.Concat(cardBrand.Where(c => !char.IsWhiteSpace(c))).ToLowerInvariant();
                var brandDimensions = _clientFunctions.GetBrandDimensions(brand);

                await _mailService.SendCardGeneratedAsync(new Emails.EmailTypes.CardGeneratedEmail
                {
                    ToEmail = user.Email,
                    CardId = userCard.Id,
                    CardType = cardType.FirstCharToUpper(),
                    CardBrand = cardBrand.FirstCharToUpper(),
                    CardNumberEndingDigits = userCard.Number.Replace("*", string.Empty).Replace(" ", string.Empty)
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW CARD] {cardType} {cardBrand} card generated",
                    Body = $"User '{user.Id}' generated a {cardType} {cardBrand} card '{userCard.Id}' at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                return Json(new
                {
                    success = true,
                    partialView = await RenderPartialViewToStringAsync("_CardsPaymentWalletAddress", new PaymentWalletAddressPartialViewModel
                    {
                        PaymentId = userCard.Id,
                        CryptocurrencyName = !string.IsNullOrWhiteSpace(paymentCryptocurrencyName) ? paymentCryptocurrencyName : ""
                    }),
                    card = new
                    {
                        id = userCard.Id,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        type = userCard.Type,
                        brand = brand + ".png",
                        brandWidth = brandDimensions.Width,
                        brandHeight = brandDimensions.Height,
                        bootstrapClasses = brandDimensions.BootstrapClasses,
                        number = userCard.Number,
                        expiration = userCard.ExpirationDate,
                        cvv = userCard.Cvv,
                        image = userCard.BackgroundImage,
                        balance = userCard.Balance,
                        activationStatus = userCard.ActivationStatus
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/confirm-payment-partial")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ConfirmPaymentPartial(string cardId)
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

                if (string.IsNullOrWhiteSpace(cardId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardId from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                var card = await _context.UserCards.SingleOrDefaultAsync(c => c.Id == cardId && c.UserId == user.Id);

                if (card == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' does not have card '{cardId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Card not found",
                        body = "Please contact the customer support.",
                        severity = "error"
                    });
                }

                var paymentCryptocurrencyName = (await _context.Cryptocurrencies.SingleOrDefaultAsync(c => c.Id == card.PaymentMethodCryptocurrencyId))?.Name ?? "";

                return Json(new
                {
                    success = true,
                    partialView = await RenderPartialViewToStringAsync("_CardsPaymentWalletAddressPartial", new PaymentWalletAddressPartialViewModel
                    {
                        PaymentId = cardId,
                        CryptocurrencyName = !string.IsNullOrWhiteSpace(paymentCryptocurrencyName) ? paymentCryptocurrencyName : ""
                    }),
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/set-sender-address")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SetSenderAddress([FromForm] PaymentWalletAddressPartialViewModel paymentWalletVm)
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

                if (!ModelState.IsValid)
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });

                var user = await _userManager.GetUserAsync(User);

                var userCard = await _context.UserCards.SingleOrDefaultAsync(c => c.Id == paymentWalletVm.PaymentId && c.UserId == user.Id && c.ActivationStatus == -1);

                if (userCard == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' does not have card '{paymentWalletVm.PaymentId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Card not found",
                        body = "Please contact the customer support.",
                        severity = "error"
                    });
                }

                userCard.PaymentWalletAddress = paymentWalletVm.WalletAddress;
                userCard.ActivationStatus = 0;

                _context.UserCards.Update(userCard);
                await _context.SaveChangesAsync();

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"Payment wallet successfully submitted for verification.",
                    Body = "",
                    Severity = "primary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-badge-check",
                    Important = false
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[WALLET ADDRESS SUBMITTED] wallet address submitted for verification of card payment",
                    Body = $"User '{user.Id}' submitted wallet address '{paymentWalletVm.WalletAddress}' for Crypto Card paymentId '{paymentWalletVm.PaymentId}' at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                return Json(new
                {
                    success = true,
                    title = $"You have successfully submitted a request for the {userCard.Brand} {userCard.Type} card!",
                    body = "Your transaction will be verified in 5-30 minutes.",
                    severity = "success",
                    cardId = userCard.Id
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/get-top-up-page")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetTopUpCapturePage(string cardId)
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

                if (string.IsNullOrWhiteSpace(cardId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardId from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                cardId = cardId.ToLower();

                var userCard = await _context.UserCards.SingleOrDefaultAsync(c => c.UserId == user.Id && c.Id == cardId);

                if (userCard == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' does not have card '{cardId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Card not found",
                        body = "You must first create a card and then top up it's balance.",
                        severity = "error"
                    });
                }

                return Json(new
                {
                    success = true,
                    partialView = await RenderPartialViewToStringAsync("_CaptureTransaction", new CaptureTransaction
                    {
                        Id = cardId,
                        AlertMessage = $"After your deposit, you will get a confirmation email with an update at {user.Email}." +
                        $"<br><br>" +
                        $"It takes 5-30 minutes to verify the deposit and add the amount to your {userCard.Type} crypto {userCard.Brand} card.",
                        HeaderText = "Choose your preferred top up cryptocurrency",
                        DropdownPlaceholderText = "Top up cryptocurrency",
                        ChosenTransactionIdLabel = "Choose the cryptocurrency with which you would like to top up your card",
                        IsBank = false,
                        ToChooseFrom_TransactionMethods = await _clientFunctions.GetSupportedCryptocurrenciesAsync()
                    })
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/get-user-transaction-qr-code")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetUserQrCodeForTransaction(string coinId, string networkId, bool isBank = false, bool isStaking = false, string validatorId = "", bool isPayPal = false)
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

                coinId = coinId.ToLower();
                networkId = networkId.ToLower();

                if (!await _context.CryptocurrencyNetworks.AsNoTracking().AnyAsync(n => n.CryptocurrencyId == coinId))
                    return Json(new
                    {
                        success = false,
                        title = "No available blockchain for the selected cryptocurrency",
                        body = "",
                        severity = "info"
                    });

                GeneratedCryptocurrencyNetworks generated = null;

                try
                {
                    generated = await _context.GeneratedCryptocurrencyNetworks.AsNoTracking().SingleOrDefaultAsync(n => n.UserId == user.Id && n.CryptocurrencyId == coinId && n.CryptocurrencyNetworkId == networkId);
                }
                catch (Exception)
                {
                    generated = default;
                }

                var network = await _context.CryptocurrencyNetworks.AsNoTracking().SingleOrDefaultAsync(n => n.CryptocurrencyId == coinId && n.Id == networkId);

                if (network == default)
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent networkId '{networkId}' for cryptocurrencyId '{coinId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                var cryptocurrency = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == network.CryptocurrencyId);

                if (cryptocurrency == default)
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' has chosen unexistent cryptocurrencyId '{network.CryptocurrencyId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                var firstTopUpText = string.Empty;
                var firstTopUpTypeText = string.Empty;

                if (!isBank && !isStaking && !isPayPal && string.IsNullOrWhiteSpace(validatorId) && !await _context.Payments.AnyAsync(p => p.UserId == user.Id && (p.Reason == "card-top-up")))
                    firstTopUpTypeText = "credit card";

                if (isBank && !isPayPal && string.IsNullOrWhiteSpace(validatorId) && !await _context.Payments.AnyAsync(p => p.UserId == user.Id && (p.Reason == "bank-deposit")))
                    firstTopUpTypeText = "bank account";

                if (isStaking && !await _context.Payments.AnyAsync(p => p.UserId == user.Id && (p.Reason == "staking-deposit")))
                    firstTopUpTypeText = "staking";

                if (isBank && isPayPal && string.IsNullOrWhiteSpace(validatorId) && !await _context.Payments.AnyAsync(p => p.UserId == user.Id && (p.Reason == "paypal-deposit")))
                    firstTopUpTypeText = "PayPal account";

                if (!string.IsNullOrWhiteSpace(firstTopUpTypeText))
                    firstTopUpText = $"<span class='font-weight-semi-bold'>Since this is your first {firstTopUpTypeText} deposit, we will add an additional 20% to the amount you deposit.</span><br><br>";

                double multiplier = isBank ? 3.5 : 2.5;
                var noticeText = $"{firstTopUpText}Please send at least {(cryptocurrency.MinimumTransactionAmount * multiplier).ToString("0.#####")} {cryptocurrency.Name} (fees included) to this address, using the {network.Name} blockchain.";

                if (isStaking)
                {
                    if (string.IsNullOrWhiteSpace(validatorId))
                    {
                        await HandleErrorJsonAsync($"User '{user.Id}' is missing validatorId from his request.");
                        return Json(new
                        {
                            success = false,
                            title = "An error occurred",
                            body = "Please refresh the page and try again or contact the customer support.",
                            severity = "error"
                        });
                    }

                    var validatorCrypto = await _context.ValidatorsCryptocurrencies.SingleOrDefaultAsync(v => v.CryptocurrencyId == cryptocurrency.Id && v.ValidatorId == validatorId);

                    if (validatorCrypto == default)
                    {
                        await HandleErrorJsonAsync($"User '{user.Id}' has chosen cryptocurrency '{network.CryptocurrencyId}' with no validators.");
                        return Json(new
                        {
                            success = false,
                            title = $"Currently, there are no available validators for {cryptocurrency.Name}",
                            body = "Please choose another cryptocurrency to stake.",
                            severity = "info"
                        });
                    }

                    noticeText = $"{firstTopUpText}Please send at least {(validatorCrypto.MinimumDepositAmount).ToString("0.#####")} {cryptocurrency.Name} (fees included) to this address, using the {network.Name} blockchain.";
                }

                return Json(new
                {
                    success = true,
                    generated = generated != default,
                    notice = noticeText,
                    qrCode = generated != default ? network.QrImage : "",
                    address = generated != default ? network.Address : "",
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/generate-network-address-for-transaction")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GenerateNetworkAddressForTransaction(string coinId, string networkId, bool isBank = false, bool isStaking = false, string validatorId = "", bool isPayPal = false)
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

                coinId = coinId.ToLower();
                networkId = networkId.ToLower();

                if (!await _context.CryptocurrencyNetworks.AsNoTracking().AnyAsync(n => n.CryptocurrencyId == coinId))
                    return Json(new
                    {
                        success = false,
                        title = "No available blockchain for the selected cryptocurrency",
                        body = "",
                        severity = "info"
                    });

                var network2 = await _context.CryptocurrencyNetworks.AsNoTracking().SingleOrDefaultAsync(n => n.CryptocurrencyId == coinId && n.Id == networkId);
                var cryptocurrency2 = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == network2.CryptocurrencyId);

                if (!_context.GeneratedCryptocurrencyNetworks.Any(c => c.UserId == user.Id && c.CryptocurrencyId == cryptocurrency2.Id && c.CryptocurrencyNetworkId == network2.Id))
                {
                    try
                    {
                        await _context.GeneratedCryptocurrencyNetworks.AddAsync(new GeneratedCryptocurrencyNetworks
                        {
                            UserId = user.Id,
                            CryptocurrencyId = cryptocurrency2.Id,
                            CryptocurrencyNetworkId = network2.Id,
                            GenerationDateTime = DateTime.UtcNow
                        });

                        await _context.SaveChangesAsync();
                    }
                    catch(Exception ex)
                    {
                        await HandleErrorJsonAsync(ex);
                    }

                    await _notificationRepository.SendAsync(new Models.Notifications
                    {
                        Id = Helper.GenerateNumbersId(),
                        ReceiverUserId = user.Id,
                        Title = $"{cryptocurrency2.Name} ({network2.Name}) address generated successfully.",
                        Body = "",
                        Severity = "primary",
                        Read = false,
                        SendingDateTime = DateTime.UtcNow,
                        Icon = "fal fa-qrcode",
                        Important = false
                    });

                    await _mailService.SendNetworkAddressGeneratedAsync(new Emails.EmailTypes.NetworkAddressGeneratedEmail
                    {
                        ToEmail = user.Email,
                        CoinName = cryptocurrency2.Name,
                        NetworkName = network2.Name,
                        Address = network2.Address
                    });

                    await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                    {
                        ToEmail = _appEmails.NotificationEmail,
                        Subject = $"[NEW ADDRESS] {cryptocurrency2.Name} ({network2.Name}) address generated",
                        Body = $"User '{user.Id}' generated a {cryptocurrency2.Name} ({network2.Name}) address generated at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                    });
                }

                var firstTopUpText = string.Empty;
                var firstTopUpTypeText = string.Empty;

                if (!isBank && !isStaking && !isPayPal && string.IsNullOrWhiteSpace(validatorId) && !await _context.Payments.AnyAsync(p => p.UserId == user.Id && (p.Reason == "card-top-up")))
                    firstTopUpTypeText = "credit card";

                if (isBank && !isPayPal && string.IsNullOrWhiteSpace(validatorId) && !await _context.Payments.AnyAsync(p => p.UserId == user.Id && (p.Reason == "bank-deposit")))
                    firstTopUpTypeText = "bank account";

                if (isStaking && !await _context.Payments.AnyAsync(p => p.UserId == user.Id && (p.Reason == "staking-deposit")))
                    firstTopUpTypeText = "staking";

                if (isBank && isPayPal && string.IsNullOrWhiteSpace(validatorId) && !await _context.Payments.AnyAsync(p => p.UserId == user.Id && (p.Reason == "paypal-deposit")))
                    firstTopUpTypeText = "PayPal account";

                if (!string.IsNullOrWhiteSpace(firstTopUpTypeText))
                    firstTopUpText = $"<span class='font-weight-semi-bold'>Since this is your first {firstTopUpTypeText} deposit, we will add an additional 20% to the amount you deposit.</span><br><br>";

                double multiplier = isBank ? 3.5 : 2.5;
                var noticeText = $"{firstTopUpText}Please send at least {(cryptocurrency2.MinimumTransactionAmount * multiplier).ToString("0.#####")} {cryptocurrency2.Name} (fees included) to this address, using the {network2.Name} blockchain.";

                if (isStaking)
                {
                    if (string.IsNullOrWhiteSpace(validatorId))
                    {
                        await HandleErrorJsonAsync($"User '{user.Id}' is missing validatorId from his request.");
                        return Json(new
                        {
                            success = false,
                            title = "An error occurred",
                            body = "Please refresh the page and try again or contact the customer support.",
                            severity = "error"
                        });
                    }

                    var validatorCrypto = await _context.ValidatorsCryptocurrencies.SingleOrDefaultAsync(v => v.CryptocurrencyId == cryptocurrency2.Id && v.ValidatorId == validatorId);

                    if (validatorCrypto == default)
                    {
                        await HandleErrorJsonAsync($"User '{user.Id}' has chosen cryptocurrency '{network2.CryptocurrencyId}' with no validators.");
                        return Json(new
                        {
                            success = false,
                            title = $"Currently, there are no available validators for {cryptocurrency2.Name}",
                            body = "Please choose another cryptocurrency to stake.",
                            severity = "info"
                        });
                    }

                    noticeText = $"{firstTopUpText}Please send at least {(validatorCrypto.MinimumDepositAmount).ToString("0.#####")} {cryptocurrency2.Name} (fees included) to this address, using the {network2.Name} blockchain.";
                }



                return Json(new
                {
                    success = true,
                    title = $"{cryptocurrency2.Name} ({network2.Name}) address generated successfully.",
                    body = "",
                    severity = "success",
                    notice = noticeText,
                    cryptocurrencyName = cryptocurrency2.Name,
                    networkName = network2.Name,
                    qrCode = network2.QrImage,
                    address = network2.Address
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/top-up")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> TopUp(string cardId)
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

                if (string.IsNullOrWhiteSpace(cardId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing cardId from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                cardId = cardId.ToLower();

                var userCard = await _context.UserCards.SingleOrDefaultAsync(c => c.Id == cardId && c.UserId == user.Id);

                if (userCard == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' does not have card '{cardId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Card not found",
                        body = "Please contact the customer support.",
                        severity = "error"
                    });
                }

                var crypto = await _context.Cryptocurrencies.SingleOrDefaultAsync(c => c.Id == userCard.PaymentMethodCryptocurrencyId);

                await _context.Payments.AddAsync(new Payments
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    Reason = "card-top-up",
                    ReasonId = cardId,
                    AmountShouldBe = (decimal)crypto.MinimumTransactionAmount,
                    PaymentMethodCryptocurrencyId = userCard.PaymentMethodCryptocurrencyId,
                    PaymentMethodCryptocurrencyNetworkId = userCard.PaymentMethodCryptocurrencyNetworkId,
                    ActivationStatus = 0,
                    CreatedDateTime = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully requested {crypto.Name} top up of your {userCard.Type} {userCard.Brand} card.",
                    Body = "",
                    Severity = "success",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "far fa-check-circle",
                    Important = false
                });

                var cardName = userCard.Brand.FirstCharToUpper();

                await _mailService.SendCaptureTransactionSuccessfulAsync(new Emails.EmailTypes.CaptureTransactionEmail
                {
                    ToEmail = user.Email,
                    Subject = $"{cardName} card top up requested successfully",
                    Header = $"You have successfully requested {crypto.Name} top up for your crypto {cardName} card",
                    Body = $"Please be patient until we verify your {crypto.Name} payment. Once verified, your card balance will be increased. " +
                    "Payments will be verified in 5-30 minutes."
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW TOP UP] Card top up request",
                    Body = $"User '{user.Id}' requested {crypto.Name} top up of his card '{userCard.Id}' at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                return Json(new
                {
                    success = true,
                    title = "Your transaction will be verified in 5-30 minutes",
                    body = $"Once your transaction is verified, your {userCard.Type} {userCard.Brand} card will be topped up and you will get a confirmation email at {user.Email}.",
                    severity = "success"
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        //APIs

        [HttpGet("/dashboard/weekly-card-top-ups")]
        public IActionResult WeeklyCardTopUps()
        {
            return Json(new
            {
                success = true,
                topUpAmount = 0
            });
        }

        [HttpGet("/dashboard/weekly-card-spendings")]
        public IActionResult WeeklyCardSpendings()
        {
            return Json(new
            {
                success = true,
                spendingAmount = 0
            });
        }
    }
}
