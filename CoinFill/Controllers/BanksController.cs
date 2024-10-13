using CoinFill.Emails;
using CoinFill.Helpers;
using CoinFill.Helpers.Models;
using CoinFill.Helpers.Providers;
using CoinFill.Implementations;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using CoinFill.PartialViewModels;
using CoinFill.ViewModels;
using CoinFill.ViewModels.BankAccountTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Controllers
{
    [Authorize(Roles = "Client")]
    public class BanksController : BaseController
    {
        private readonly AppEmails _appEmails;
        private readonly ICustomClientFunctionsProvider _clientFunctions;

        public BanksController(IConfiguration configuration,
            IErrorLogger errors,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager) : base(errors, mailService, contextAccessor, notificationRepository, userManager)
        {
            _appEmails = configuration.GetSection("AppEmails").Get<AppEmails>();
            _clientFunctions = new CustomClientFunctionsProvider(contextAccessor);
        }

        [HttpGet("/dashboard/bank-accounts/{bankId?}")]
        public async Task<IActionResult> BankAccounts(string bankId = null)
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

                var banksPageVm = new BankAccountsPageViewModel();

                foreach (var bankAccount in _context.UserBankAccounts.AsNoTracking().Where(b => b.UserId == user.Id).OrderBy(b => b.AddedDateTime))
                {
                    if (string.IsNullOrWhiteSpace(bankAccount.BicSwift))
                        bankAccount.BicSwift = "N/A - not required";

                    if (bankAccount.Currency == "EUR")
                    {
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                    else if(bankAccount.Currency == "INR")
                    {
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                    else if(bankAccount.Currency == "CZK")
                    {
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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
                        banksPageVm.BankAccounts.Add(new BankAccountViewModel
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

                if (!string.IsNullOrWhiteSpace(bankId))
                {
                    banksPageVm.BankIdToDeposit = bankId;

                    if (bankId.ToLower() != "add-new")
                    {
                        var bankToDeposit = banksPageVm.BankAccounts.SingleOrDefault(b => b.Id == bankId.ToLower());
                        if (bankToDeposit != default)
                        {
                            banksPageVm.StartDeposit = true;
                        }
                        else
                        {
                            ViewBag.BankDepositHeaderMessage = "Requested bank account not found";
                            ViewBag.BankDepositBodyMessage = "The bank account you want to deposit to does not exist.";
                            ViewBag.BankDepositSeverity = "info";
                        }
                    }
                    else
                    {
                        ViewBag.AddNew = true;
                    }
                }

                return View(banksPageVm);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/get-bank-withdraw-page")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetBankWithdrawCapturePage(string bankId)
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

                if (string.IsNullOrWhiteSpace(bankId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing bankId from his request.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                bankId = bankId.ToLower();

                var userBankAccount = await _context.UserBankAccounts.AsNoTracking().SingleOrDefaultAsync(c => c.UserId == user.Id && c.Id == bankId);

                if (userBankAccount == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' does not have card '{bankId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Bank account not found",
                        body = "You must first add a bank account and then send cryptocurrencies to it.",
                        severity = "error"
                    });
                }

                string accountType = "bank account";
                string accountLabel = "bank";

                string fee = string.Empty;

                if (userBankAccount.Currency == "EUR") fee = BankAccountDepositFeeProvider.EUR;
                else if (userBankAccount.Currency == "USD") fee = BankAccountDepositFeeProvider.USD;
                else if (userBankAccount.Currency == "GBP") fee = BankAccountDepositFeeProvider.GBP;
                else if (userBankAccount.Currency == "CHF") fee = BankAccountDepositFeeProvider.CHF;
                else if (userBankAccount.Currency == "CNY")
                {
                    accountType = "card";
                    accountLabel = "card";
                    fee = BankAccountDepositFeeProvider.HKD;
                }
                else if (userBankAccount.Currency == "HKD") fee = BankAccountDepositFeeProvider.HKD;
                else if (userBankAccount.Currency == "RUB") fee = BankAccountDepositFeeProvider.RUB;
                else if (userBankAccount.Currency == "IRR")
                {
                    accountType = "card";
                    accountLabel = "card";
                    fee = BankAccountDepositFeeProvider.IRR;
                }
                else if (userBankAccount.Currency == "KRW") fee = BankAccountDepositFeeProvider.KRW;
                else if (userBankAccount.Currency == "CAD") fee = BankAccountDepositFeeProvider.CAD;
                else if (userBankAccount.Currency == "AUD") fee = BankAccountDepositFeeProvider.AUD;
                else if (userBankAccount.Currency == "NOK") fee = BankAccountDepositFeeProvider.NOK;
                else if (userBankAccount.Currency == "DKK") fee = BankAccountDepositFeeProvider.DKK;
                else if (userBankAccount.Currency == "SEK") fee = BankAccountDepositFeeProvider.SEK;
                else if (userBankAccount.Currency == "INR") fee = BankAccountDepositFeeProvider.INR;
                else if (userBankAccount.Currency == "HUF") fee = BankAccountDepositFeeProvider.HUF;
                else if (userBankAccount.Currency == "RSD") fee = BankAccountDepositFeeProvider.RSD;
                else if (userBankAccount.Currency == "BAM") fee = BankAccountDepositFeeProvider.BAM;
                else if (userBankAccount.Currency == "PLN") fee = BankAccountDepositFeeProvider.PLN;
                else if (userBankAccount.Currency == "UAH") fee = BankAccountDepositFeeProvider.UAH;
                else if (userBankAccount.Currency == "CZK") fee = BankAccountDepositFeeProvider.CZK;
                else if (userBankAccount.Currency == "RON") fee = BankAccountDepositFeeProvider.RON;
                else if (userBankAccount.Currency == "BGN") fee = BankAccountDepositFeeProvider.BGN;
                else if (userBankAccount.Currency == "TRY") fee = BankAccountDepositFeeProvider.TRY;
                else if (userBankAccount.Currency == "AED") fee = BankAccountDepositFeeProvider.AED;
                else if (userBankAccount.Currency == "MXN") fee = BankAccountDepositFeeProvider.MXN;
                else if (userBankAccount.Currency == "ILS") fee = BankAccountDepositFeeProvider.ILS;
                else if (userBankAccount.Currency == "PKR") fee = BankAccountDepositFeeProvider.PKR;
                else if (userBankAccount.Currency == "MAD") fee = BankAccountDepositFeeProvider.MAD;
                else fee = "0.20";

                return Json(new
                {
                    success = true,
                    partialView = await RenderPartialViewToStringAsync("_CaptureBankDeposit", new CaptureTransaction
                    {
                        Id = bankId,
                        AlertMessage = $"The cryptocurrency amount you send will be converted to {userBankAccount.Currency} and sent to your {accountType} ({fee}% fee)." +
                        $"<br><br>" +
                        $"It takes 30 minutes to verify the deposit, do the conversion and send the amount to your {userBankAccount.Currency} {accountType}.",
                        HeaderText = $"Choose your preferred cryptocurrency for {accountLabel} deposit",
                        DropdownPlaceholderText = "Preferred cryptocurrency",
                        ChosenTransactionIdLabel = $"Choose the cryptocurrency with which you would like to make a deposit into your {userBankAccount.Currency} {accountType}",
                        IsBank = true,
                        ToChooseFrom_TransactionMethods = await _clientFunctions.GetSupportedCryptocurrenciesAsync()
                    })
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-eur-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddEurBankAccount(AddBankAccountEur eurAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {eurAccount.EurAccountHolderFirstName} - LastName: {eurAccount.EurAccountHolderLastName} - Currency: {eurAccount.EurAccountCurrency} - BIC/SWIFT: {eurAccount.EurBicSwift} - IBAN: {eurAccount.EurIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(eurAccount.EurBicSwift))
                    eurAccount.EurBicSwift = "";

                eurAccount.EurAccountCurrency = "EUR";

                if(await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == eurAccount.EurAccountCurrency && b.BicSwift.ToLower() == eurAccount.EurBicSwift.ToLower() && b.BankAccountNumber.ToLower() == eurAccount.EurIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{eurAccount.EurAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = eurAccount.EurAccountHolderFirstName,
                    LastName = eurAccount.EurAccountHolderLastName,
                    Currency = eurAccount.EurAccountCurrency,
                    BankAccountNumber = eurAccount.EurIBAN,
                    BicSwift = eurAccount.EurBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.EUR
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "EUR bank account added successfully",
                    body = "Now you can send cryptocurrencies to your EUR bank account and they will be converted to EUR upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.EUR,
                        firstName = eurAccount.EurAccountHolderFirstName,
                        lastName = eurAccount.EurAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-usd-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddUsdBankAccount(AddBankAccountUsd usdAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {usdAccount.UsdAccountHolderFirstName} - LastName: {usdAccount.UsdAccountHolderLastName} - Currency: {usdAccount.UsdAccountCurrency} - RoutingNumber: {usdAccount.UsdRoutingNumber} - BankAccountNumber: {usdAccount.UsdBankAccountNumber}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                usdAccount.UsdAccountCurrency = "USD";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == usdAccount.UsdAccountCurrency && b.RoutingNumber.ToLower() == usdAccount.UsdRoutingNumber.ToLower() && b.BankAccountNumber.ToLower() == usdAccount.UsdBankAccountNumber.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{usdAccount.UsdAccountCurrency} account with this Routing Number and Account Number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = usdAccount.UsdAccountHolderFirstName,
                    LastName = usdAccount.UsdAccountHolderLastName,
                    Currency = usdAccount.UsdAccountCurrency,
                    RoutingNumber = usdAccount.UsdRoutingNumber,
                    BankAccountNumber = usdAccount.UsdBankAccountNumber,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.USD
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "USD bank account added successfully",
                    body = "Now you can send cryptocurrencies to your USD bank account and they will be converted to USD upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.USD,
                        firstName = usdAccount.UsdAccountHolderFirstName,
                        lastName = usdAccount.UsdAccountHolderLastName,
                        currency = bankAccount.Currency,
                        routingNumber = bankAccount.RoutingNumber,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-gbp-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddGbpBankAccount(AddBankAccountGbp gbpAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {gbpAccount.GbpAccountHolderFirstName} - LastName: {gbpAccount.GbpAccountHolderLastName} - Currency: {gbpAccount.GbpAccountCurrency} - BIC/SWIFT: {gbpAccount.GbpBicSwift} - IBAN: {gbpAccount.GbpIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(gbpAccount.GbpBicSwift))
                    gbpAccount.GbpBicSwift = "";

                gbpAccount.GbpAccountCurrency = "GBP";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == gbpAccount.GbpAccountCurrency && b.BicSwift.ToLower() == gbpAccount.GbpBicSwift.ToLower() && b.BankAccountNumber.ToLower() == gbpAccount.GbpIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{gbpAccount.GbpAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = gbpAccount.GbpAccountHolderFirstName,
                    LastName = gbpAccount.GbpAccountHolderLastName,
                    Currency = gbpAccount.GbpAccountCurrency,
                    BankAccountNumber = gbpAccount.GbpIBAN,
                    BicSwift = gbpAccount.GbpBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.GBP
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "GBP bank account added successfully",
                    body = "Now you can send cryptocurrencies to your GBP bank account and they will be converted to GBP upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.GBP,
                        firstName = gbpAccount.GbpAccountHolderFirstName,
                        lastName = gbpAccount.GbpAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-chf-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddChfBankAccount(AddBankAccountChf chfAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {chfAccount.ChfAccountHolderFirstName} - LastName: {chfAccount.ChfAccountHolderLastName} - Currency: {chfAccount.ChfAccountCurrency} - BIC/SWIFT: {chfAccount.ChfBicSwift} - IBAN: {chfAccount.ChfIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(chfAccount.ChfBicSwift))
                    chfAccount.ChfBicSwift = "";

                chfAccount.ChfAccountCurrency = "CHF";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == chfAccount.ChfAccountCurrency && b.BicSwift.ToLower() == chfAccount.ChfBicSwift.ToLower() && b.BankAccountNumber.ToLower() == chfAccount.ChfIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{chfAccount.ChfAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = chfAccount.ChfAccountHolderFirstName,
                    LastName = chfAccount.ChfAccountHolderLastName,
                    Currency = chfAccount.ChfAccountCurrency,
                    BankAccountNumber = chfAccount.ChfIBAN,
                    BicSwift = chfAccount.ChfBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.CHF
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "CHF bank account added successfully",
                    body = "Now you can send cryptocurrencies to your CHF bank account and they will be converted to CHF upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.CHF,
                        firstName = chfAccount.ChfAccountHolderFirstName,
                        lastName = chfAccount.ChfAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-cny-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddCnyBankAccount(AddBankAccountCny cnyAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {cnyAccount.CnyAccountHolderFirstName} - LastName: {cnyAccount.CnyAccountHolderLastName} - Currency: {cnyAccount.CnyAccountCurrency} - IBAN: {cnyAccount.CnyIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                cnyAccount.CnyAccountCurrency = "CNY";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == cnyAccount.CnyAccountCurrency && b.BankAccountNumber.ToLower() == cnyAccount.CnyIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{cnyAccount.CnyAccountCurrency} account with this Card Number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = cnyAccount.CnyAccountHolderFirstName,
                    LastName = cnyAccount.CnyAccountHolderLastName,
                    Currency = cnyAccount.CnyAccountCurrency,
                    BankAccountNumber = cnyAccount.CnyIBAN,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendCardAccountGeneratedAsync(new Emails.EmailTypes.CardAccountGeneratedEmail
                {
                    CardId = bankAccount.Id,
                    ToEmail = user.Email,
                    CardAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} UnionPay card added successfully",
                    CardAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.CNY
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW CARD ACCOUNT] {bankAccount.Currency} card added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} card at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} UnionPay card.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "CNY UnionPay card added successfully",
                    body = "Now you can send cryptocurrencies to your CNY card and they will be converted to CNY upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.CNY,
                        firstName = cnyAccount.CnyAccountHolderFirstName,
                        lastName = cnyAccount.CnyAccountHolderLastName,
                        currency = bankAccount.Currency,
                        cardAccountNumber = bankAccount.BankAccountNumber,
                        accountType = "Card",
                        idLabel = "Card"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-hkd-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddHkdBankAccount(AddBankAccountHkd hkdAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {hkdAccount.HkdAccountHolderFirstName} - LastName: {hkdAccount.HkdAccountHolderLastName} - Currency: {hkdAccount.HkdAccountCurrency} - BankCode: {hkdAccount.HkdBankCode} - BankAccountNumber: {hkdAccount.HkdBankAccountNumber}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(hkdAccount.HkdBankCode))
                    hkdAccount.HkdBankCode = "";

                hkdAccount.HkdAccountCurrency = "HKD";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == hkdAccount.HkdAccountCurrency && b.BicSwift.ToLower() == hkdAccount.HkdBankCode.ToLower() && b.BankAccountNumber.ToLower() == hkdAccount.HkdBankAccountNumber.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{hkdAccount.HkdAccountCurrency} account with this Bank/Clearing Code and Bank Account Number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = hkdAccount.HkdAccountHolderFirstName,
                    LastName = hkdAccount.HkdAccountHolderLastName,
                    Currency = hkdAccount.HkdAccountCurrency,
                    BankAccountNumber = hkdAccount.HkdBankAccountNumber,
                    BicSwift = hkdAccount.HkdBankCode,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.HKD
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "HKD bank account added successfully",
                    body = "Now you can send cryptocurrencies to your HKD bank account and they will be converted to HKD upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.HKD,
                        firstName = hkdAccount.HkdAccountHolderFirstName,
                        lastName = hkdAccount.HkdAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-rub-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddRubBankAccount(AddBankAccountRub rubAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {rubAccount.RubAccountHolderFirstName} - LastName: {rubAccount.RubAccountHolderLastName} - Currency: {rubAccount.RubAccountCurrency} - BIC/SWIFT: {rubAccount.RubBicSwift} - IBAN: {rubAccount.RubIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(rubAccount.RubBicSwift))
                    rubAccount.RubBicSwift = "";

                rubAccount.RubAccountCurrency = "RUB";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == rubAccount.RubAccountCurrency && b.BicSwift.ToLower() == rubAccount.RubBicSwift.ToLower() && b.BankAccountNumber.ToLower() == rubAccount.RubIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{rubAccount.RubAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = rubAccount.RubAccountHolderFirstName,
                    LastName = rubAccount.RubAccountHolderLastName,
                    Currency = rubAccount.RubAccountCurrency,
                    BankAccountNumber = rubAccount.RubIBAN,
                    BicSwift = rubAccount.RubBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.RUB
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "RUB bank account added successfully",
                    body = "Now you can send cryptocurrencies to your RUB bank account and they will be converted to RUB upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.RUB,
                        firstName = rubAccount.RubAccountHolderFirstName,
                        lastName = rubAccount.RubAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-irr-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddIrrBankAccount(AddBankAccountIrr irrAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {irrAccount.IrrAccountHolderFirstName} - LastName: {irrAccount.IrrAccountHolderLastName} - Currency: {irrAccount.IrrAccountCurrency} - IBAN: {irrAccount.IrrIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                irrAccount.IrrAccountCurrency = "IRR";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == irrAccount.IrrAccountCurrency && b.BankAccountNumber.ToLower() == irrAccount.IrrIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{irrAccount.IrrAccountCurrency} account with this Card Number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = irrAccount.IrrAccountHolderFirstName,
                    LastName = irrAccount.IrrAccountHolderLastName,
                    Currency = irrAccount.IrrAccountCurrency,
                    BankAccountNumber = irrAccount.IrrIBAN,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendCardAccountGeneratedAsync(new Emails.EmailTypes.CardAccountGeneratedEmail
                {
                    CardId = bankAccount.Id,
                    ToEmail = user.Email,
                    CardAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} card added successfully",
                    CardAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.IRR
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW CARD ACCOUNT] {bankAccount.Currency} card added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} card at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} card.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "IRR card added successfully",
                    body = "Now you can send cryptocurrencies to your IRR card and they will be converted to IRR upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.IRR,
                        firstName = irrAccount.IrrAccountHolderFirstName,
                        lastName = irrAccount.IrrAccountHolderLastName,
                        currency = bankAccount.Currency,
                        cardAccountNumber = bankAccount.BankAccountNumber,
                        accountType = "Card",
                        idLabel = "Card"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-krw-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddKrwBankAccount(AddBankAccountKrw krwAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {krwAccount.KrwAccountHolderFirstName} - LastName: {krwAccount.KrwAccountHolderLastName} - Currency: {krwAccount.KrwAccountCurrency} - BankCode: {krwAccount.KrwBankCode} - BankAccountNumber: {krwAccount.KrwBankAccountNumber}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(krwAccount.KrwBankCode))
                    krwAccount.KrwBankCode = "";

                krwAccount.KrwAccountCurrency = "KRW";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == krwAccount.KrwAccountCurrency && b.BicSwift.ToLower() == krwAccount.KrwBankCode.ToLower() && b.BankAccountNumber.ToLower() == krwAccount.KrwBankAccountNumber.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{krwAccount.KrwAccountCurrency} account with this Bank Code/Name and Bank Account Number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = krwAccount.KrwAccountHolderFirstName,
                    LastName = krwAccount.KrwAccountHolderLastName,
                    Currency = krwAccount.KrwAccountCurrency,
                    BankAccountNumber = krwAccount.KrwBankAccountNumber,
                    BicSwift = krwAccount.KrwBankCode,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.KRW
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "KRW bank account added successfully",
                    body = "Now you can send cryptocurrencies to your KRW bank account and they will be converted to KRW upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.KRW,
                        firstName = krwAccount.KrwAccountHolderFirstName,
                        lastName = krwAccount.KrwAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-cad-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddCadBankAccount(AddBankAccountCad cadAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {cadAccount.CadAccountHolderFirstName} - LastName: {cadAccount.CadAccountHolderLastName} - Currency: {cadAccount.CadAccountCurrency} - TransitNumber: {cadAccount.CadTransitNumber} - BankAccountNumber: {cadAccount.CadBankAccountNumber} - InstitutionNumber: {cadAccount.CadInstitutionNumber}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                cadAccount.CadAccountCurrency = "CAD";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == cadAccount.CadAccountCurrency && b.TransitNumber.ToLower() == cadAccount.CadTransitNumber.ToLower() && b.BankAccountNumber.ToLower() == cadAccount.CadBankAccountNumber.ToLower() && b.InstitutionNumber.ToLower() == cadAccount.CadInstitutionNumber.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{cadAccount.CadAccountCurrency} account with this Transit Number, Institution Number and Account Number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = cadAccount.CadAccountHolderFirstName,
                    LastName = cadAccount.CadAccountHolderLastName,
                    Currency = cadAccount.CadAccountCurrency,
                    TransitNumber = cadAccount.CadTransitNumber,
                    InstitutionNumber = cadAccount.CadInstitutionNumber,
                    BankAccountNumber = cadAccount.CadBankAccountNumber,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.CAD
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "CAD bank account added successfully",
                    body = "Now you can send cryptocurrencies to your CAD bank account and they will be converted to CAD upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.CAD,
                        firstName = cadAccount.CadAccountHolderFirstName,
                        lastName = cadAccount.CadAccountHolderLastName,
                        currency = bankAccount.Currency,
                        transitNumber = bankAccount.TransitNumber,
                        institutionNumber = bankAccount.InstitutionNumber,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                    }
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-aud-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddAudBankAccount(AddBankAccountAud audAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {audAccount.AudAccountHolderFirstName} - LastName: {audAccount.AudAccountHolderLastName} - Currency: {audAccount.AudAccountCurrency} - BSB/BIC/SWIFT: {audAccount.AudBsb} - BankAccountNumber: {audAccount.AudBankAccountNumber}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(audAccount.AudBsb))
                    audAccount.AudBsb = "";

                audAccount.AudAccountCurrency = "AUD";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == audAccount.AudAccountCurrency && b.BicSwift.ToLower() == audAccount.AudBsb.ToLower() && b.BankAccountNumber.ToLower() == audAccount.AudBankAccountNumber.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{audAccount.AudAccountCurrency} account with this BSB/BIC/SWIFT and Account Number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = audAccount.AudAccountHolderFirstName,
                    LastName = audAccount.AudAccountHolderLastName,
                    Currency = audAccount.AudAccountCurrency,
                    BankAccountNumber = audAccount.AudBankAccountNumber,
                    BicSwift = audAccount.AudBsb,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.AUD
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "AUD bank account added successfully",
                    body = "Now you can send cryptocurrencies to your AUD bank account and they will be converted to AUD upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.AUD,
                        firstName = audAccount.AudAccountHolderFirstName,
                        lastName = audAccount.AudAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-nok-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddNokBankAccount(AddBankAccountNok nokAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {nokAccount.NokAccountHolderFirstName} - LastName: {nokAccount.NokAccountHolderLastName} - Currency: {nokAccount.NokAccountCurrency} - BIC/SWIFT: {nokAccount.NokBicSwift} - IBAN: {nokAccount.NokIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(nokAccount.NokBicSwift))
                    nokAccount.NokBicSwift = "";

                nokAccount.NokAccountCurrency = "NOK";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == nokAccount.NokAccountCurrency && b.BicSwift.ToLower() == nokAccount.NokBicSwift.ToLower() && b.BankAccountNumber.ToLower() == nokAccount.NokIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{nokAccount.NokAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = nokAccount.NokAccountHolderFirstName,
                    LastName = nokAccount.NokAccountHolderLastName,
                    Currency = nokAccount.NokAccountCurrency,
                    BankAccountNumber = nokAccount.NokIBAN,
                    BicSwift = nokAccount.NokBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.NOK
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "NOK bank account added successfully",
                    body = "Now you can send cryptocurrencies to your NOK bank account and they will be converted to NOK upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.NOK,
                        firstName = nokAccount.NokAccountHolderFirstName,
                        lastName = nokAccount.NokAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-dkk-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddDkkBankAccount(AddBankAccountDkk dkkAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {dkkAccount.DkkAccountHolderFirstName} - LastName: {dkkAccount.DkkAccountHolderLastName} - Currency: {dkkAccount.DkkAccountCurrency} - BIC/SWIFT: {dkkAccount.DkkBicSwift} - IBAN: {dkkAccount.DkkIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(dkkAccount.DkkBicSwift))
                    dkkAccount.DkkBicSwift = "";

                dkkAccount.DkkAccountCurrency = "DKK";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == dkkAccount.DkkAccountCurrency && b.BicSwift.ToLower() == dkkAccount.DkkBicSwift.ToLower() && b.BankAccountNumber.ToLower() == dkkAccount.DkkIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{dkkAccount.DkkAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = dkkAccount.DkkAccountHolderFirstName,
                    LastName = dkkAccount.DkkAccountHolderLastName,
                    Currency = dkkAccount.DkkAccountCurrency,
                    BankAccountNumber = dkkAccount.DkkIBAN,
                    BicSwift = dkkAccount.DkkBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.DKK
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "DKK bank account added successfully",
                    body = "Now you can send cryptocurrencies to your DKK bank account and they will be converted to DKK upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.DKK,
                        firstName = dkkAccount.DkkAccountHolderFirstName,
                        lastName = dkkAccount.DkkAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-sek-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddSekBankAccount(AddBankAccountSek sekAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {sekAccount.SekAccountHolderFirstName} - LastName: {sekAccount.SekAccountHolderLastName} - Currency: {sekAccount.SekAccountCurrency} - BIC/SWIFT: {sekAccount.SekBicSwift} - IBAN: {sekAccount.SekIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(sekAccount.SekBicSwift))
                    sekAccount.SekBicSwift = "";

                sekAccount.SekAccountCurrency = "SEK";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == sekAccount.SekAccountCurrency && b.BicSwift.ToLower() == sekAccount.SekBicSwift.ToLower() && b.BankAccountNumber.ToLower() == sekAccount.SekIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{sekAccount.SekAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = sekAccount.SekAccountHolderFirstName,
                    LastName = sekAccount.SekAccountHolderLastName,
                    Currency = sekAccount.SekAccountCurrency,
                    BankAccountNumber = sekAccount.SekIBAN,
                    BicSwift = sekAccount.SekBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.SEK
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "SEK bank account added successfully",
                    body = "Now you can send cryptocurrencies to your SEK bank account and they will be converted to SEK upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.SEK,
                        firstName = sekAccount.SekAccountHolderFirstName,
                        lastName = sekAccount.SekAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-inr-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddInrBankAccount(AddBankAccountInr inrAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {inrAccount.InrAccountHolderFirstName} - LastName: {inrAccount.InrAccountHolderLastName} - Currency: {inrAccount.InrAccountCurrency} - IFSC: {inrAccount.InrIFSC} - BankAccountNumber: {inrAccount.InrBankAccountNumber}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                inrAccount.InrAccountCurrency = "INR";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == inrAccount.InrAccountCurrency && b.BicSwift.ToLower() == inrAccount.InrIFSC.ToLower() && b.BankAccountNumber.ToLower() == inrAccount.InrBankAccountNumber.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{inrAccount.InrAccountCurrency} account with this IFSC and Account Number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = inrAccount.InrAccountHolderFirstName,
                    LastName = inrAccount.InrAccountHolderLastName,
                    Currency = inrAccount.InrAccountCurrency,
                    BankAccountNumber = inrAccount.InrBankAccountNumber,
                    BicSwift = inrAccount.InrIFSC,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.INR
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "INR bank account added successfully",
                    body = "Now you can send cryptocurrencies to your INR bank account and they will be converted to INR upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.INR,
                        firstName = inrAccount.InrAccountHolderFirstName,
                        lastName = inrAccount.InrAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        ifsc = bankAccount.BicSwift
                    }
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-huf-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddHufBankAccount(AddBankAccountHuf hufAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {hufAccount.HufAccountHolderFirstName} - LastName: {hufAccount.HufAccountHolderLastName} - Currency: {hufAccount.HufAccountCurrency} - BIC/SWIFT: {hufAccount.HufBicSwift} - IBAN: {hufAccount.HufIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(hufAccount.HufBicSwift))
                    hufAccount.HufBicSwift = "";

                hufAccount.HufAccountCurrency = "HUF";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == hufAccount.HufAccountCurrency && b.BicSwift.ToLower() == hufAccount.HufBicSwift.ToLower() && b.BankAccountNumber.ToLower() == hufAccount.HufIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{hufAccount.HufAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = hufAccount.HufAccountHolderFirstName,
                    LastName = hufAccount.HufAccountHolderLastName,
                    Currency = hufAccount.HufAccountCurrency,
                    BankAccountNumber = hufAccount.HufIBAN,
                    BicSwift = hufAccount.HufBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.HUF
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "HUF bank account added successfully",
                    body = "Now you can send cryptocurrencies to your HUF bank account and they will be converted to HUF upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.HUF,
                        firstName = hufAccount.HufAccountHolderFirstName,
                        lastName = hufAccount.HufAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-rsd-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddRsdBankAccount(AddBankAccountRsd rsdAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {rsdAccount.RsdAccountHolderFirstName} - LastName: {rsdAccount.RsdAccountHolderLastName} - Currency: {rsdAccount.RsdAccountCurrency} - BIC/SWIFT: {rsdAccount.RsdBicSwift} - IBAN: {rsdAccount.RsdIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(rsdAccount.RsdBicSwift))
                    rsdAccount.RsdBicSwift = "";

                rsdAccount.RsdAccountCurrency = "RSD";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == rsdAccount.RsdAccountCurrency && b.BicSwift.ToLower() == rsdAccount.RsdBicSwift.ToLower() && b.BankAccountNumber.ToLower() == rsdAccount.RsdIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{rsdAccount.RsdAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = rsdAccount.RsdAccountHolderFirstName,
                    LastName = rsdAccount.RsdAccountHolderLastName,
                    Currency = rsdAccount.RsdAccountCurrency,
                    BankAccountNumber = rsdAccount.RsdIBAN,
                    BicSwift = rsdAccount.RsdBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.RSD
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "RSD bank account added successfully",
                    body = "Now you can send cryptocurrencies to your RSD bank account and they will be converted to RSD upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.RSD,
                        firstName = rsdAccount.RsdAccountHolderFirstName,
                        lastName = rsdAccount.RsdAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-bam-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddBamBankAccount(AddBankAccountBam bamAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {bamAccount.BamAccountHolderFirstName} - LastName: {bamAccount.BamAccountHolderLastName} - Currency: {bamAccount.BamAccountCurrency} - BIC/SWIFT: {bamAccount.BamBicSwift} - IBAN: {bamAccount.BamIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(bamAccount.BamBicSwift))
                    bamAccount.BamBicSwift = "";

                bamAccount.BamAccountCurrency = "BAM";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == bamAccount.BamAccountCurrency && b.BicSwift.ToLower() == bamAccount.BamBicSwift.ToLower() && b.BankAccountNumber.ToLower() == bamAccount.BamIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{bamAccount.BamAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = bamAccount.BamAccountHolderFirstName,
                    LastName = bamAccount.BamAccountHolderLastName,
                    Currency = bamAccount.BamAccountCurrency,
                    BankAccountNumber = bamAccount.BamIBAN,
                    BicSwift = bamAccount.BamBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.BAM
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "BAM bank account added successfully",
                    body = "Now you can send cryptocurrencies to your BAM bank account and they will be converted to BAM upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.BAM,
                        firstName = bamAccount.BamAccountHolderFirstName,
                        lastName = bamAccount.BamAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-pln-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddPlnBankAccount(AddBankAccountPln plnAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {plnAccount.PlnAccountHolderFirstName} - LastName: {plnAccount.PlnAccountHolderLastName} - Currency: {plnAccount.PlnAccountCurrency} - BIC/SWIFT: {plnAccount.PlnBicSwift} - IBAN: {plnAccount.PlnIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(plnAccount.PlnBicSwift))
                    plnAccount.PlnBicSwift = "";

                plnAccount.PlnAccountCurrency = "PLN";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == plnAccount.PlnAccountCurrency && b.BicSwift.ToLower() == plnAccount.PlnBicSwift.ToLower() && b.BankAccountNumber.ToLower() == plnAccount.PlnIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{plnAccount.PlnAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = plnAccount.PlnAccountHolderFirstName,
                    LastName = plnAccount.PlnAccountHolderLastName,
                    Currency = plnAccount.PlnAccountCurrency,
                    BankAccountNumber = plnAccount.PlnIBAN,
                    BicSwift = plnAccount.PlnBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.PLN
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "PLN bank account added successfully",
                    body = "Now you can send cryptocurrencies to your PLN bank account and they will be converted to PLN upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.PLN,
                        firstName = plnAccount.PlnAccountHolderFirstName,
                        lastName = plnAccount.PlnAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-uah-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddUahBankAccount(AddBankAccountUah uahAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {uahAccount.UahAccountHolderFirstName} - LastName: {uahAccount.UahAccountHolderLastName} - Currency: {uahAccount.UahAccountCurrency} - BIC/SWIFT: {uahAccount.UahBicSwift} - IBAN: {uahAccount.UahIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(uahAccount.UahBicSwift))
                    uahAccount.UahBicSwift = "";

                uahAccount.UahAccountCurrency = "UAH";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == uahAccount.UahAccountCurrency && b.BicSwift.ToLower() == uahAccount.UahBicSwift.ToLower() && b.BankAccountNumber.ToLower() == uahAccount.UahIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{uahAccount.UahAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = uahAccount.UahAccountHolderFirstName,
                    LastName = uahAccount.UahAccountHolderLastName,
                    Currency = uahAccount.UahAccountCurrency,
                    BankAccountNumber = uahAccount.UahIBAN,
                    BicSwift = uahAccount.UahBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.UAH
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "UAH bank account added successfully",
                    body = "Now you can send cryptocurrencies to your UAH bank account and they will be converted to UAH upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.UAH,
                        firstName = uahAccount.UahAccountHolderFirstName,
                        lastName = uahAccount.UahAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-czk-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddCzkBankAccount(AddBankAccountCzk czkAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {czkAccount.CzkAccountHolderFirstName} - LastName: {czkAccount.CzkAccountHolderLastName} - Currency: {czkAccount.CzkAccountCurrency} - BIC/SWIFT: {czkAccount.CzkBicSwift} - IBAN: {czkAccount.CzkIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(czkAccount.CzkBicSwift))
                    czkAccount.CzkBicSwift = "";

                czkAccount.CzkAccountCurrency = "CZK";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == czkAccount.CzkAccountCurrency && b.BicSwift.ToLower() == czkAccount.CzkBicSwift.ToLower() && b.BankAccountNumber.ToLower() == czkAccount.CzkIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{czkAccount.CzkAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = czkAccount.CzkAccountHolderFirstName,
                    LastName = czkAccount.CzkAccountHolderLastName,
                    Currency = czkAccount.CzkAccountCurrency,
                    BankAccountNumber = czkAccount.CzkIBAN,
                    BicSwift = czkAccount.CzkBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.CZK
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "CZK bank account added successfully",
                    body = "Now you can send cryptocurrencies to your CZK bank account and they will be converted to CZK upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.CZK,
                        firstName = czkAccount.CzkAccountHolderFirstName,
                        lastName = czkAccount.CzkAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-ron-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddRonBankAccount(AddBankAccountRon ronAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {ronAccount.RonAccountHolderFirstName} - LastName: {ronAccount.RonAccountHolderLastName} - Currency: {ronAccount.RonAccountCurrency} - BIC/SWIFT: {ronAccount.RonBicSwift} - IBAN: {ronAccount.RonIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(ronAccount.RonBicSwift))
                    ronAccount.RonBicSwift = "";

                ronAccount.RonAccountCurrency = "RON";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == ronAccount.RonAccountCurrency && b.BicSwift.ToLower() == ronAccount.RonBicSwift.ToLower() && b.BankAccountNumber.ToLower() == ronAccount.RonIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{ronAccount.RonAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = ronAccount.RonAccountHolderFirstName,
                    LastName = ronAccount.RonAccountHolderLastName,
                    Currency = ronAccount.RonAccountCurrency,
                    BankAccountNumber = ronAccount.RonIBAN,
                    BicSwift = ronAccount.RonBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.RON
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "RON bank account added successfully",
                    body = "Now you can send cryptocurrencies to your RON bank account and they will be converted to RON upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.RON,
                        firstName = ronAccount.RonAccountHolderFirstName,
                        lastName = ronAccount.RonAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-bgn-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddBgnBankAccount(AddBankAccountBgn bgnAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {bgnAccount.BgnAccountHolderFirstName} - LastName: {bgnAccount.BgnAccountHolderLastName} - Currency: {bgnAccount.BgnAccountCurrency} - BIC/SWIFT: {bgnAccount.BgnBicSwift} - IBAN: {bgnAccount.BgnIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(bgnAccount.BgnBicSwift))
                    bgnAccount.BgnBicSwift = "";

                bgnAccount.BgnAccountCurrency = "BGN";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == bgnAccount.BgnAccountCurrency && b.BicSwift.ToLower() == bgnAccount.BgnBicSwift.ToLower() && b.BankAccountNumber.ToLower() == bgnAccount.BgnIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{bgnAccount.BgnAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = bgnAccount.BgnAccountHolderFirstName,
                    LastName = bgnAccount.BgnAccountHolderLastName,
                    Currency = bgnAccount.BgnAccountCurrency,
                    BankAccountNumber = bgnAccount.BgnIBAN,
                    BicSwift = bgnAccount.BgnBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.BGN
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "BGN bank account added successfully",
                    body = "Now you can send cryptocurrencies to your BGN bank account and they will be converted to BGN upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.BGN,
                        firstName = bgnAccount.BgnAccountHolderFirstName,
                        lastName = bgnAccount.BgnAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-try-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddTryBankAccount(AddBankAccountTry tryAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {tryAccount.TryAccountHolderFirstName} - LastName: {tryAccount.TryAccountHolderLastName} - Currency: {tryAccount.TryAccountCurrency} - BIC/SWIFT: {tryAccount.TryBicSwift} - IBAN: {tryAccount.TryIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(tryAccount.TryBicSwift))
                    tryAccount.TryBicSwift = "";

                tryAccount.TryAccountCurrency = "TRY";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == tryAccount.TryAccountCurrency && b.BicSwift.ToLower() == tryAccount.TryBicSwift.ToLower() && b.BankAccountNumber.ToLower() == tryAccount.TryIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{tryAccount.TryAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = tryAccount.TryAccountHolderFirstName,
                    LastName = tryAccount.TryAccountHolderLastName,
                    Currency = tryAccount.TryAccountCurrency,
                    BankAccountNumber = tryAccount.TryIBAN,
                    BicSwift = tryAccount.TryBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.TRY
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "TRY bank account added successfully",
                    body = "Now you can send cryptocurrencies to your TRY bank account and they will be converted to TRY upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.TRY,
                        firstName = tryAccount.TryAccountHolderFirstName,
                        lastName = tryAccount.TryAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-aed-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddAedBankAccount(AddBankAccountAed aedAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {aedAccount.AedAccountHolderFirstName} - LastName: {aedAccount.AedAccountHolderLastName} - Currency: {aedAccount.AedAccountCurrency} - BIC/SWIFT: {aedAccount.AedBicSwift} - IBAN: {aedAccount.AedIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(aedAccount.AedBicSwift))
                    aedAccount.AedBicSwift = "";

                aedAccount.AedAccountCurrency = "AED";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == aedAccount.AedAccountCurrency && b.BicSwift.ToLower() == aedAccount.AedBicSwift.ToLower() && b.BankAccountNumber.ToLower() == aedAccount.AedIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{aedAccount.AedAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = aedAccount.AedAccountHolderFirstName,
                    LastName = aedAccount.AedAccountHolderLastName,
                    Currency = aedAccount.AedAccountCurrency,
                    BankAccountNumber = aedAccount.AedIBAN,
                    BicSwift = aedAccount.AedBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.AED
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "AED bank account added successfully",
                    body = "Now you can send cryptocurrencies to your AED bank account and they will be converted to AED upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.AED,
                        firstName = aedAccount.AedAccountHolderFirstName,
                        lastName = aedAccount.AedAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-ils-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddIlsBankAccount(AddBankAccountIls ilsAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {ilsAccount.IlsAccountHolderFirstName} - LastName: {ilsAccount.IlsAccountHolderLastName} - Currency: {ilsAccount.IlsAccountCurrency} - BIC/SWIFT: {ilsAccount.IlsBicSwift} - IBAN: {ilsAccount.IlsIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(ilsAccount.IlsBicSwift))
                    ilsAccount.IlsBicSwift = "";

                ilsAccount.IlsAccountCurrency = "ILS";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == ilsAccount.IlsAccountCurrency && b.BicSwift.ToLower() == ilsAccount.IlsBicSwift.ToLower() && b.BankAccountNumber.ToLower() == ilsAccount.IlsIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{ilsAccount.IlsAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = ilsAccount.IlsAccountHolderFirstName,
                    LastName = ilsAccount.IlsAccountHolderLastName,
                    Currency = ilsAccount.IlsAccountCurrency,
                    BankAccountNumber = ilsAccount.IlsIBAN,
                    BicSwift = ilsAccount.IlsBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.ILS
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "ILS bank account added successfully",
                    body = "Now you can send cryptocurrencies to your ILS bank account and they will be converted to ILS upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.ILS,
                        firstName = ilsAccount.IlsAccountHolderFirstName,
                        lastName = ilsAccount.IlsAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-mxn-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddMxnBankAccount(AddBankAccountMxn mxnAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {mxnAccount.MxnAccountHolderFirstName} - LastName: {mxnAccount.MxnAccountHolderLastName} - Currency: {mxnAccount.MxnAccountCurrency} - IBAN: {mxnAccount.MxnIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                mxnAccount.MxnAccountCurrency = "MXN";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == mxnAccount.MxnAccountCurrency && b.BankAccountNumber.ToLower() == mxnAccount.MxnIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{mxnAccount.MxnAccountCurrency} account with this CLABE number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = mxnAccount.MxnAccountHolderFirstName,
                    LastName = mxnAccount.MxnAccountHolderLastName,
                    Currency = mxnAccount.MxnAccountCurrency,
                    BankAccountNumber = mxnAccount.MxnIBAN,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.MXN
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "MXN bank account added successfully",
                    body = "Now you can send cryptocurrencies to your MXN bank account and they will be converted to MXN upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.MXN,
                        firstName = mxnAccount.MxnAccountHolderFirstName,
                        lastName = mxnAccount.MxnAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-pkr-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddPkrBankAccount(AddBankAccountPkr pkrAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {pkrAccount.PkrAccountHolderFirstName} - LastName: {pkrAccount.PkrAccountHolderLastName} - Currency: {pkrAccount.PkrAccountCurrency} - BIC/SWIFT: {pkrAccount.PkrBicSwift} - IBAN: {pkrAccount.PkrIBAN}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                if (string.IsNullOrWhiteSpace(pkrAccount.PkrBicSwift))
                    pkrAccount.PkrBicSwift = "";

                pkrAccount.PkrAccountCurrency = "PKR";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == pkrAccount.PkrAccountCurrency && b.BicSwift.ToLower() == pkrAccount.PkrBicSwift.ToLower() && b.BankAccountNumber.ToLower() == pkrAccount.PkrIBAN.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{pkrAccount.PkrAccountCurrency} account with this BIC/SWIFT and IBAN already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = pkrAccount.PkrAccountHolderFirstName,
                    LastName = pkrAccount.PkrAccountHolderLastName,
                    Currency = pkrAccount.PkrAccountCurrency,
                    BankAccountNumber = pkrAccount.PkrIBAN,
                    BicSwift = pkrAccount.PkrBicSwift,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.PKR
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "PKR bank account added successfully",
                    body = "Now you can send cryptocurrencies to your PKR bank account and they will be converted to PKR upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.PKR,
                        firstName = pkrAccount.PkrAccountHolderFirstName,
                        lastName = pkrAccount.PkrAccountHolderLastName,
                        currency = bankAccount.Currency,
                        bankAccountNumber = bankAccount.BankAccountNumber,
                        bicSwift = !string.IsNullOrWhiteSpace(bankAccount.BicSwift) ? bankAccount.BicSwift : "N/A - not required"
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/add-mad-bank-account")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddMadBankAccount(AddBankAccountMad madAccount)
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
                    await HandleErrorAsync($"User '{user.Id}' did not enter valid ModelState data. FirstName: {madAccount.MadAccountHolderFirstName} - LastName: {madAccount.MadAccountHolderLastName} - Currency: {madAccount.MadAccountCurrency} - RIB: {madAccount.MadRib}.");
                    return Json(new
                    {
                        success = false,
                        title = "Fill all the required fields",
                        body = "",
                        severity = "info"
                    });
                }

                madAccount.MadAccountCurrency = "MAD";

                if (await _context.UserBankAccounts.AsNoTracking().AnyAsync(b => b.UserId == user.Id && b.Currency == madAccount.MadAccountCurrency && b.BankAccountNumber.ToLower() == madAccount.MadRib.ToLower()))
                {
                    return Json(new
                    {
                        success = false,
                        title = $"{madAccount.MadAccountCurrency} account with this RIB number already exists",
                        body = "",
                        severity = "info"
                    });
                }

                var bankAccount = new UserBankAccounts
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    FirstName = madAccount.MadAccountHolderFirstName,
                    LastName = madAccount.MadAccountHolderLastName,
                    Currency = madAccount.MadAccountCurrency,
                    BankAccountNumber = madAccount.MadRib,
                    AddedDateTime = DateTime.UtcNow
                };

                await _context.UserBankAccounts.AddAsync(bankAccount);

                await _context.SaveChangesAsync();

                await _mailService.SendBankAccountGeneratedAsync(new Emails.EmailTypes.BankAccountGeneratedEmail
                {
                    BankId = bankAccount.Id,
                    ToEmail = user.Email,
                    BankAccountCurrency = bankAccount.Currency,
                    Subject = $"{bankAccount.Currency} bank account added successfully",
                    BankAccountEndingDigits = bankAccount.BankAccountNumber.Substring(bankAccount.BankAccountNumber.Length - 4),
                    DepositFee = BankAccountDepositFeeProvider.MAD
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW BANK ACCOUNT] {bankAccount.Currency} bank account added",
                    Body = $"User '{user.Id}' added a {bankAccount.Currency} bank account at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully added the {bankAccount.Currency} bank account.",
                    Body = "",
                    Severity = "secondary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-university",
                    Important = false
                });

                return Json(new
                {
                    success = true,
                    title = "MAD bank account added successfully",
                    body = "Now you can send cryptocurrencies to your MAD bank account and they will be converted to MAD upon arrival.",
                    severity = "success",
                    bank = new
                    {
                        id = bankAccount.Id,
                        fee = BankAccountDepositFeeProvider.MAD,
                        firstName = madAccount.MadAccountHolderFirstName,
                        lastName = madAccount.MadAccountHolderLastName,
                        currency = bankAccount.Currency,
                        rib = bankAccount.BankAccountNumber
                    }
                });
            }
            catch (Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/bank-accounts/bank-deposit")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> BankDeposit(string bankId, string coinId, string networkId)
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

                if (string.IsNullOrWhiteSpace(bankId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' is missing bankId from his request.");
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

                bankId = bankId.ToLower();
                coinId = coinId.ToLower();
                networkId = networkId.ToLower();

                string accountType = "bank account";
                string accountLabel = "bank";

                var userBank = await _context.UserBankAccounts.SingleOrDefaultAsync(c => c.Id == bankId && c.UserId == user.Id);

                if (userBank == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' does not have a bank account '{bankId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Bank account not found",
                        body = "Please contact the customer support.",
                        severity = "error"
                    });
                }

                var crypto = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == coinId);

                await _context.Payments.AddAsync(new Payments
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    Reason = "bank-deposit",
                    ReasonId = bankId,
                    AmountShouldBe = (decimal)crypto.MinimumTransactionAmount,
                    PaymentMethodCryptocurrencyId = coinId,
                    PaymentMethodCryptocurrencyNetworkId = networkId,
                    ActivationStatus = 0,
                    CreatedDateTime = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                if(userBank.Currency == "IRR")
                {
                    accountType = "card";
                    accountLabel = "card";
                }

                await _notificationRepository.SendAsync(new Models.Notifications
                {
                    Id = Helper.GenerateNumbersId(),
                    ReceiverUserId = user.Id,
                    Title = $"You have successfully requested {crypto.Name} deposit into your {userBank.Currency} {accountType}.",
                    Body = "",
                    Severity = "success",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "far fa-hand-holding-usd",
                    Important = false
                });

                await _mailService.SendCaptureTransactionSuccessfulAsync(new Emails.EmailTypes.CaptureTransactionEmail
                {
                    ToEmail = user.Email,
                    Subject = $"{userBank.Currency} {accountType} deposit requested successfully",
                    Header = $"You have successfully requested {crypto.Name} deposit into your {userBank.Currency} {accountType}",
                    Body = $"Please be patient until we verify your {crypto.Name} payment. Once verified, the deposit will be sent to your {userBank.Currency} {accountType}."
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW {accountLabel.ToUpper()} DEPOSIT] {accountType} deposit request",
                    Body = $"User '{user.Id}' requested a {crypto.Name} deposit to {accountLabel} account '{userBank.Id}' at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                return Json(new
                {
                    success = true,
                    title = "Your transaction will be verified in 30 minutes",
                    body = $"Once your deposit is verified, the amount will be sent to your {userBank.Currency} {accountType} and you will get a confirmation email at {user.Email}. " +
                    $"It takes 1-3 business day(s) for the deposit to get to your {accountType}.",
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
