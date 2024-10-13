using ClosedXML.Excel;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Controllers
{
    [Authorize(Roles = "Client")]
    public class StakingController : BaseController
    {
        private readonly AppEmails _appEmails;
        private readonly ICustomClientFunctionsProvider _clientFunctions;

        public StakingController(IConfiguration configuration,
            IErrorLogger errors,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager) : base(errors, mailService, contextAccessor, notificationRepository, userManager)
        {
            _appEmails = configuration.GetSection("AppEmails").Get<AppEmails>();
            _clientFunctions = new CustomClientFunctionsProvider(contextAccessor);
        }

        [AllowAnonymous]
        [HttpGet("/high-apy-staking")]
        public async Task<IActionResult> HighApyStaking()
        {
            try
            {
                ShowToastOnThisPageIfSet();

                var stakingVm = new StakingViewModel();

                foreach (var tempItem in (await _context.Cryptocurrencies
                                                .AsNoTracking()
                                                .ToListAsync())
                                                .GroupJoin(_context.ValidatorsCryptocurrencies.AsNoTracking(),
                                                           crypto => crypto.Id,
                                                           val => val.CryptocurrencyId,
                                                           (cryptocurrency, validators) => new
                                                           {
                                                               CryptocurrencyId = cryptocurrency.Id,
                                                               CryptocurrencyName = cryptocurrency.Name,
                                                               CryptocurrencyColor = cryptocurrency.Color,
                                                               CryptocurrencyIcon = cryptocurrency.Icon,
                                                               CryptoCurrencyOrderNumber = cryptocurrency.OrderNumber,
                                                               MaxApy = validators.Any() ? validators.Max(validators => validators.Apy) : 0,
                                                               ValidatorsIds = validators.OrderByDescending(v => v.Apy).Select(v => v.ValidatorId)
                                                           }))
                {
                    var cryptoValidators = new CryptocurrencyValidatorsViewModel
                    {
                        CryptocurrencyId = tempItem.CryptocurrencyId,
                        CryptocurrencyName = tempItem.CryptocurrencyName,
                        CryptocurrencyColor = tempItem.CryptocurrencyColor,
                        CryptocurrencyIcon = tempItem.CryptocurrencyIcon,
                        CryptocurrencyOrderNumber = tempItem.CryptoCurrencyOrderNumber,
                        MaxApy = tempItem.MaxApy
                    };

                    foreach (var validatorId in tempItem.ValidatorsIds)
                    {
                        var validator = await _context.Validators.AsNoTracking().SingleOrDefaultAsync(val => val.Id == validatorId);
                        if (validator != default)
                        {
                            cryptoValidators.Validators.Add(new ValidatorShowcaseViewModel
                            {
                                Id = validator.Id,
                                Photo = validator.Photo,
                                Name = validator.Name,
                            });
                        }
                    }

                    stakingVm.Cryptocurrencies.Add(cryptoValidators);
                }

                stakingVm.Cryptocurrencies = stakingVm.Cryptocurrencies.OrderBy(c => c.CryptocurrencyOrderNumber).ToList();

                return View(stakingVm);
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/dashboard/staking/")]
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

                var stakingVm = new StakingViewModel();

                foreach (var tempItem in (await _context.Cryptocurrencies
                                                .AsNoTracking()
                                                .ToListAsync())
                                                .GroupJoin(_context.ValidatorsCryptocurrencies.AsNoTracking(),
                                                           crypto => crypto.Id,
                                                           val => val.CryptocurrencyId,
                                                           (cryptocurrency, validators) => new
                                                           {
                                                               CryptocurrencyId = cryptocurrency.Id,
                                                               CryptocurrencyName = cryptocurrency.Name,
                                                               CryptocurrencyColor = cryptocurrency.Color,
                                                               CryptocurrencyIcon = cryptocurrency.Icon,
                                                               CryptoCurrencyOrderNumber = cryptocurrency.OrderNumber,
                                                               MaxApy = validators.Any() ? validators.Max(validators => validators.Apy) : 0,
                                                               ValidatorsIds = validators.OrderByDescending(v => v.Apy).Select(v => v.ValidatorId)
                                                           }))
                {
                    var cryptoValidators = new CryptocurrencyValidatorsViewModel
                    {
                        CryptocurrencyId = tempItem.CryptocurrencyId,
                        CryptocurrencyName = tempItem.CryptocurrencyName,
                        CryptocurrencyColor = tempItem.CryptocurrencyColor,
                        CryptocurrencyIcon = tempItem.CryptocurrencyIcon,
                        CryptocurrencyOrderNumber = tempItem.CryptoCurrencyOrderNumber,
                        MaxApy = tempItem.MaxApy
                    };

                    foreach(var validatorId in tempItem.ValidatorsIds)
                    {
                        var validator = await _context.Validators.AsNoTracking().SingleOrDefaultAsync(val => val.Id == validatorId);
                        if(validator != default)
                        {
                            cryptoValidators.Validators.Add(new ValidatorShowcaseViewModel
                            {
                                Id = validator.Id,
                                Photo = validator.Photo,
                                Name = validator.Name,
                            });
                        }
                    }

                    stakingVm.Cryptocurrencies.Add(cryptoValidators);
                }

                stakingVm.Cryptocurrencies = stakingVm.Cryptocurrencies.OrderBy(c => c.CryptocurrencyOrderNumber).ToList();

                return View(stakingVm);
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/dashboard/staking/my")]
        public async Task<IActionResult> MyStakes()
        {
            try
            {
                var emailStatus = await _clientFunctions.HasConfirmedEmailAsync();
                if (emailStatus == EmailConfirmationStatus.Error)
                    throw new GeneralException("unable to load user", signOutUser: true);
                else if (emailStatus == EmailConfirmationStatus.NotConfirmed)
                    return RedirectToAction("ConfirmEmail", "Authorization", new { Area = "" });

                ShowToastOnThisPageIfSet();

                var myStakeVm = new List<CryptocurrencyTvlViewModel>();

                foreach (var tempItem in (await _context.Cryptocurrencies
                                                .AsNoTracking()
                                                .ToListAsync())
                                                .GroupJoin(_context.ValidatorsCryptocurrencies.AsNoTracking(),
                                                           crypto => crypto.Id,
                                                           val => val.CryptocurrencyId,
                                                           (cryptocurrency, validators) => new
                                                           {
                                                               CryptocurrencyId = cryptocurrency.Id,
                                                               CryptocurrencyName = cryptocurrency.Name,
                                                               CryptocurrencyColor = cryptocurrency.Color,
                                                               CryptocurrencyIcon = cryptocurrency.Icon,
                                                               CryptoCurrencyOrderNumber = cryptocurrency.OrderNumber,
                                                               MaxApy = validators.Any() ? validators.Max(validators => validators.Apy) : 0,
                                                               TotalTvl = validators.Sum(v => v.TotalStaked)
                                                           }))
                {
                    myStakeVm.Add(new CryptocurrencyTvlViewModel
                    {
                        CryptocurrencyId = tempItem.CryptocurrencyId,
                        CryptocurrencyName = tempItem.CryptocurrencyName,
                        CryptocurrencyColor = tempItem.CryptocurrencyColor,
                        CryptocurrencyIcon = tempItem.CryptocurrencyIcon,
                        CryptocurrencyOrderNumber = tempItem.CryptoCurrencyOrderNumber,
                        MaxApy = tempItem.MaxApy,
                        MyTvl = "0",
                        TotalTvl = tempItem.TotalTvl.ToString()
                    });
                }

                myStakeVm = myStakeVm.OrderBy(c => c.CryptocurrencyOrderNumber).ToList();

                return View(myStakeVm);
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpPost("/dashboard/staking/unstake")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Unstake(string coinId)
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

                var crypto = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == coinId);

                if(crypto == default)
                {

                }

                return Json(new
                {
                    success = true,
                    noCryptoToUnstake = true,
                    title = $"You don't have any {crypto.Name} to unstake",
                    body = $"First, you need to stake some {crypto.Name} before attempting to unstake it.",
                    redirectToStake = Url.Action("Index", "Staking", new { Area = "" }),
                    btnStakeText = $"Stake {crypto.Name}",
                    cryptocurrencyId = coinId,
                    severity = "info"
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/staking/get-validators")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetValidators(string coinId)
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

                var cryptocurrency = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == coinId);

                if(cryptocurrency == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' chose an unexistend cryptocurrency '{coinId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Selected cryptocurrency unavailable",
                        body = "Please choose another cryptocurrency to stake.",
                        severity = "error"
                    });
                }

                var validators = (from cryptoVal in await _context.ValidatorsCryptocurrencies.AsNoTracking().Where(c => c.CryptocurrencyId == coinId).ToListAsync()
                                  join validator in _context.Validators.AsNoTracking()
                                  on cryptoVal.ValidatorId equals validator.Id
                                  select new
                                  {
                                      validatorId = validator.Id,
                                      validatorPhoto = validator.Photo,
                                      validatorName = validator.Name,
                                      apy = cryptoVal.Apy,
                                      totalStaked = cryptoVal.TotalStaked
                                  }).OrderByDescending(v => v.apy);

                return Json(new
                {
                    success = true,
                    cryptocurrencyId = cryptocurrency.Id,
                    cryptocurrencyIcon = cryptocurrency.Icon,
                    cryptocurrencyName = cryptocurrency.Name,
                    cryptocurrencyColor = cryptocurrency.Color,
                    totalStaked = validators.Sum(v => v.totalStaked).ToString("0.##"),
                    validators = validators
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/staking/get-validator-details")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetValidatorsDetails(string validatorId, string coinId)
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

                validatorId = validatorId.ToLower();
                coinId = coinId.ToLower();

                var cryptocurrency = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == coinId);

                if (cryptocurrency == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' chose an unexistend cryptocurrency '{coinId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Selected cryptocurrency unavailable",
                        body = "Please choose another cryptocurrency to stake.",
                        severity = "error"
                    });
                }

                var validator = (from cryptoVal in await _context.ValidatorsCryptocurrencies.AsNoTracking().Where(v => v.ValidatorId == validatorId && v.CryptocurrencyId == coinId).ToListAsync()
                                 join val in _context.Validators.AsNoTracking()
                                 on cryptoVal.ValidatorId equals val.Id
                                 select new
                                 {
                                     validatorId = val.Id,
                                     validatorPhoto = val.Photo,
                                     validatorName = val.Name,
                                     cryptocurrencyId = cryptoVal.CryptocurrencyId,
                                     apy = cryptoVal.Apy,
                                     totalStaked = cryptoVal.TotalStaked,
                                     unlockTimeHours = cryptoVal.UnlockTimeHours,
                                     minimumDepositAmount = cryptoVal.MinimumDepositAmount
                                 }).SingleOrDefault();

                return Json(new
                {
                    success = true,
                    cryptocurrencyId = cryptocurrency.Id,
                    cryptocurrencyIcon = cryptocurrency.Icon,
                    cryptocurrencyName = cryptocurrency.Name,
                    cryptocurrencyColor = cryptocurrency.Color,
                    userCurrentStake = 0,
                    validator = validator
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/staking/get-validator-deposit-page")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetValidatorDepositCapturePage(string validatorId, string coinId)
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

                validatorId = validatorId.ToLower();
                coinId = coinId.ToLower();

                var validatorCrypto = (from valCrypto in (await _context.ValidatorsCryptocurrencies.AsNoTracking().Where(v => v.ValidatorId == validatorId && v.CryptocurrencyId == coinId).ToListAsync())
                                       join val in _context.Validators.AsNoTracking()
                                       on valCrypto.ValidatorId equals val.Id
                                       join crypto in _context.Cryptocurrencies.AsNoTracking()
                                       on valCrypto.CryptocurrencyId equals crypto.Id
                                       select new
                                       {
                                           Validator = val,
                                           ValidatorCryptocurrency = valCrypto,
                                           Cryptocurrency = crypto
                                       }).SingleOrDefault();

                if(validatorCrypto == default)
                {
                    await HandleErrorJsonAsync($" User '{user.Id}' selected a validator '{validatorId}' but that validator does not have cryptocurrency '{coinId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                validatorCrypto.ValidatorCryptocurrency.Validator = validatorCrypto.Validator;
                validatorCrypto.ValidatorCryptocurrency.Cryptocurrency = validatorCrypto.Cryptocurrency;

                return Json(new
                {
                    success = true,
                    partialView = await RenderPartialViewToStringAsync("_CaptureStakingDeposit", new CaptureStakingDepositTransaction(validatorCrypto.ValidatorCryptocurrency)
                    {
                        AlertMessage = $"After your deposit, you will get a confirmation email with an update at {user.Email}." +
                        $"<br><br>" +
                        $"It takes 5-30 minutes to verify the deposit and stake your {validatorCrypto.Cryptocurrency.Name} with {validatorCrypto.Validator.Name} validator for {validatorCrypto.ValidatorCryptocurrency.Apy}% APY.",
                        HeaderText = $"Stake your {validatorCrypto.Cryptocurrency.Name} with {validatorCrypto.Validator.Name} validator",
                        DropdownPlaceholderText = $"{validatorCrypto.Cryptocurrency.Name} blockchain",
                        ChosenNetworkIdLabel = $"Choose your preferred {validatorCrypto.Cryptocurrency.Name} blockchain / network",
                        ToChooseFrom_Networks = await _clientFunctions.GetNetworksForCryptocurrencyAsync(coinId)
                    })
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpPost("/dashboard/staking/stake-deposit")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> StakeDeposit(string validatorId, string coinId, string networkId)
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

                validatorId = validatorId.ToLower();
                coinId = coinId.ToLower();
                networkId = networkId.ToLower();

                var valCrypto = (from cryptoVal in (await _context.ValidatorsCryptocurrencies.AsNoTracking().Where(v => v.ValidatorId == validatorId && v.CryptocurrencyId == coinId).ToListAsync())
                                 join crypto in _context.Cryptocurrencies.AsNoTracking()
                                 on cryptoVal.CryptocurrencyId equals crypto.Id
                                 join validator in _context.Validators.AsNoTracking()
                                 on cryptoVal.ValidatorId equals validator.Id
                                 select new
                                 {
                                     Cryptocurrency = crypto,
                                     ValidatorCryptocurrency = cryptoVal,
                                     Validator = validator
                                 }).SingleOrDefault();

                if (valCrypto == default)
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' ValidatorCryptocurrency with ValidatorId '{validatorId}' and cryptocurrencyId '{coinId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "An error occurred",
                        body = "Please refresh the page and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                if (!await _context.CryptocurrencyNetworks.AsNoTracking().AnyAsync(n => n.CryptocurrencyId == coinId && n.Id == networkId))
                {
                    await HandleErrorJsonAsync($"User '{user.Id}' chose unexistent networkId '{networkId}'.");
                    return Json(new
                    {
                        success = false,
                        title = "Selected network does not exist",
                        body = "Please choose another network and try again or contact the customer support.",
                        severity = "error"
                    });
                }

                await _context.Payments.AddAsync(new Payments
                {
                    Id = Helper.GenerateNumbersId(),
                    UserId = user.Id,
                    Reason = "staking-deposit",
                    ReasonId = validatorId,
                    AmountShouldBe = (decimal)valCrypto.ValidatorCryptocurrency.MinimumDepositAmount,
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
                    Title = $"It takes 5-30 minutes to verify your deposit and stake your {valCrypto.Cryptocurrency.Name} with {valCrypto.Validator.Name} validator.",
                    Body = "",
                    Severity = "primary",
                    Read = false,
                    SendingDateTime = DateTime.UtcNow,
                    Icon = "fas fa-layer-group",
                    Important = false
                });

                await _mailService.SendCaptureStakingTransactionSuccessfulAsync(new Emails.EmailTypes.CaptureTransactionEmail
                {
                    ToEmail = user.Email,
                    Subject = $"{valCrypto.Cryptocurrency.Name} staking deposit requested successfully",
                    Header = $"You have successfully requested {valCrypto.Cryptocurrency.Name} staking deposit for {valCrypto.ValidatorCryptocurrency.Apy}% APY",
                    Body = $"Please be patient until we verify your {valCrypto.Cryptocurrency.Name} staking deposit. Once verified, your {valCrypto.Cryptocurrency.Name} will be staked with {valCrypto.Validator.Name} validator. " +
                    "Staking deposits will be verified in 5-30 minutes."
                });

                await _mailService.SendEmailAsync(new Emails.EmailTypes.Email
                {
                    ToEmail = _appEmails.NotificationEmail,
                    Subject = $"[NEW STAKING] Staking deposit request",
                    Body = $"User '{user.Id}' requested {valCrypto.Cryptocurrency.Name} staking deposit with validator '{valCrypto.Validator.Name}' at {DateTime.UtcNow} UTC (LOCAL: {DateTime.Now})"
                });

                return Json(new
                {
                    success = true,
                    title = "Staking request sent successfully",
                    body = $"It takes 5-30 minutes to verify your deposit and stake your {valCrypto.Cryptocurrency.Name} with {valCrypto.Validator.Name} validator for {valCrypto.ValidatorCryptocurrency.Apy}% APY.",
                    severity = "success"
                });
            }
            catch(Exception e)
            {
                return await HandleErrorJsonAsync(e);
            }
        }

        [HttpGet("/dashboard/staking/get-staking-statistics")]
        public async Task<IActionResult> ExportDataInExcel()
        {
            try
            {
                return GenerateStakingStatisticsExcel("staking-earnings.xlsx", await _context.Cryptocurrencies.ToListAsync());
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        private FileResult GenerateStakingStatisticsExcel(string fileName, IEnumerable<Cryptocurrencies> cryptos)
        {
            var dataTable = new DataTable("CryptocurrencyStakingRewards");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("ID"),
                new DataColumn("Name"),
                new DataColumn("Total staked"),
                new DataColumn("Earned from staking"),
                new DataColumn("USD earnings from staking")
            });

            foreach(var crypto in cryptos)
            {
                dataTable.Rows.Add(crypto.Id, crypto.Name, "0", "0", "0");
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
        }
    }
}
