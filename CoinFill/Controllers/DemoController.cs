using CoinFill.Emails;
using CoinFill.Implementations;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using CoinFill.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinFill.Helpers.Providers;

namespace CoinFill.Controllers
{
    public class DemoController : BaseController
    {
        private readonly ICustomClientFunctionsProvider _clientFunctions;

        public DemoController(IErrorLogger errors,
            IMailService mailService,
            IHttpContextAccessor contextAccessor,
            INotificationRepository notificationRepository,
            UserManager<CustomClient> userManager) : base(errors, mailService, contextAccessor, notificationRepository, userManager)
        {
            _clientFunctions = new CustomClientFunctionsProvider(contextAccessor);
        }

        [HttpGet("/demo/dashboard/account")]
        public async Task<IActionResult> Profile()
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

        [HttpGet("/demo/dashboard/account/edit-address")]
        public async Task<IActionResult> EditAddress()
        {
            try
            {
                ShowToastOnThisPageIfSet();

                return View(new EditAddressViewModel
                {
                    FullName = "Leonard Cooper",
                    Street = "South Lancaster",
                    HouseNumber = "219",
                    City = "Brooklyn, NY",
                    PostalCode = "11215",
                    Chosen_CountryId = "United States of America",
                    ToChooseFrom_Countries = _clientFunctions.GetSupportedCountries()
                });
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/demo/dashboard")]
        public async Task<IActionResult> Dashboard()
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

        [HttpGet("/demo/dashboard/cards")]
        public async Task<IActionResult> Cards()
        {
            try
            {
                ShowToastOnThisPageIfSet();

                return View(SupportedCountriesProvider.SupportedCountries.Count);
            }
            catch(Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/demo/dashboard/bank-accounts")]
        public async Task<IActionResult> BankAccounts()
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

        [HttpGet("/demo/dashboard/staking")]
        public async Task<IActionResult> Staking()
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
                                                               ValidatorsIds = validators/*.OrderByDescending(v => v.Apy)*/.Select(v => v.ValidatorId)
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
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/demo/dashboard/staking/my")]
        public async Task<IActionResult> MyStakes()
        {
            try
            {
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
                                                               MyTvl = cryptocurrency.Name == "BTC" ? 1.02 : cryptocurrency.MinimumTransactionAmount * 15.23,
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
                        MyTvl = tempItem.MyTvl.ToString("0.##"),
                        TotalTvl = tempItem.TotalTvl.ToString()
                    });
                }

                myStakeVm = myStakeVm.OrderBy(c => c.CryptocurrencyOrderNumber).ToList();

                return View(myStakeVm);
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }

        [HttpGet("/demo/dashboard/paypal/transfer")]
        public async Task<IActionResult> TransferToPayPal()
        {
            try
            {
                ShowToastOnThisPageIfSet();

                return View(new PayPalViewModel());
            }
            catch (Exception e)
            {
                return await HandleErrorAsync(e);
            }
        }
    }
}
