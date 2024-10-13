using CoinFill.Helpers.Models;
using CoinFill.Helpers.Providers;
using CoinFill.Interfaces;
using CoinFill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoinFill.Implementations
{
    public class CustomClientFunctionsProvider : ICustomClientFunctionsProvider
    {
        //private readonly IHttpContextAccessor _contextAccessor;
        //private readonly IErrorLogger _errors;
        private readonly CoinFillContext _context;
        private readonly ClaimsPrincipal User;
        private readonly UserManager<CustomClient> _userManager;

        public CustomClientFunctionsProvider(IHttpContextAccessor contextAccessor)
        {
            //_contextAccessor = contextAccessor;
            //_errors = contextAccessor.HttpContext.RequestServices.GetRequiredService<IErrorLogger>();
            _context = new CoinFillContext();
            User = contextAccessor.HttpContext.User;
            _userManager = contextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<CustomClient>>();
        }

        public async Task<EmailConfirmationStatus> HasConfirmedEmailAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return EmailConfirmationStatus.Confirmed;

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return EmailConfirmationStatus.Error;

            if (string.IsNullOrWhiteSpace(user.Email))
                return EmailConfirmationStatus.Error;

            if (await _userManager.IsEmailConfirmedAsync(user))
                return EmailConfirmationStatus.Confirmed;

            return EmailConfirmationStatus.NotConfirmed;
        }

        public CardBrandDimensions GetBrandDimensions(string brand)
        {
            var d = new CardBrandDimensions();

            if(brand == "visa")
            {
                d.Width = 50;
                d.Height = 30;
                d.BootstrapClasses = "pb-1";
            }
            else if(brand == "mastercard")
            {
                d.Width = 50;
                d.Height = 35;
                d.BootstrapClasses = "pb-0";
            }
            else if(brand == "americanexpress")
            {
                d.Width = 75;
                d.Height = 30;
                d.BootstrapClasses = "pb-1";
            }
            else if(brand == "unionpay")
            {
                d.Width = 50;
                d.Height = 30;
                d.BootstrapClasses = "pb-1";
            }
            else if (brand == "discover")
            {
                d.Width = 110;
                d.Height = 20;
                d.BootstrapClasses = "pb-2 mb-1";
            }
            else if (brand == "jcb")
            {
                d.Width = 40;
                d.Height = 30;
                d.BootstrapClasses = "pb-1";
            }
            else if (brand == "elo")
            {
                d.Width = 65;
                d.Height = 25;
                d.BootstrapClasses = "pb-2";
            }

            return d;
        }

        public bool CardTypeExists(string cardType)
        {
            cardType = cardType.ToLowerInvariant();
            return cardType == "virtual" || cardType == "physical";
        }

        public bool CardBrandExists(string cardBrand)
        {
            cardBrand = cardBrand.ToLowerInvariant();
            return cardBrand == "visa" || cardBrand == "mastercard" ||
                cardBrand == "american express" || cardBrand == "unionpay" ||
                cardBrand == "discover" || cardBrand == "jcb" || cardBrand == "elo";
        }

        public async Task<List<SelectListItem>> GetSupportedCryptocurrenciesAsync()
        {
            var dropdown = new List<SelectListItem>();

            foreach (var crypto in (await _context.Cryptocurrencies.AsNoTracking().ToListAsync()).OrderBy(c => c.OrderNumber))
            {
                dropdown.Add(new SelectListItem
                {
                    Value = crypto.Id,
                    Text = crypto.Icon + "|" + crypto.Name + "|" + crypto.Color
                });
            }

            return dropdown;
        }

        public async Task<SelectListItem> GetSupportedCryptocurrencyAsync(string validatorId, string coinId)
        {
            var validatorCrypto = await _context.ValidatorsCryptocurrencies.AsNoTracking().SingleOrDefaultAsync(v => v.ValidatorId == validatorId && v.CryptocurrencyId == coinId);

            if (validatorCrypto == default)
                throw new System.Exception($"No entity in table ValidatorsCryptocurrencies with ValidatorId={validatorId} and CryptocurrencyId={coinId}.");

            var crypto = await _context.Cryptocurrencies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == validatorCrypto.CryptocurrencyId);

            return new SelectListItem
            {
                Value = crypto.Id,
                Text = crypto.Icon + "|" + crypto.Name + "|" + crypto.Color,
                Selected = true
            };
        }

        public async Task<List<SelectListItem>> GetNetworksForCryptocurrencyAsync(string coinId)
        {
            var dropdown = new List<SelectListItem>();

            foreach (var network in (await _context.CryptocurrencyNetworks.AsNoTracking().Where(n => n.CryptocurrencyId == coinId).ToListAsync()).OrderBy(n => n.OrderNumber))
            {
                dropdown.Add(new SelectListItem
                {
                    Value = network.Id,
                    Text = network.Name
                });
            }

            return dropdown;
        }

        public List<SelectListItem> GetSupportedCountries()
        {
            var dropdown = new List<SelectListItem>();

            foreach (var country in SupportedCountriesProvider.SupportedCountries)
            {
                dropdown.Add(new SelectListItem
                {
                    Value = country.Name,
                    Text = country.Icon + "|" + country.Name
                });
            }

            return dropdown;
        }
    }
}
