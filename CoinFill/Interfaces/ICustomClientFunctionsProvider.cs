using CoinFill.Helpers.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinFill.Interfaces
{
    public enum EmailConfirmationStatus
    {
        Confirmed,
        NotConfirmed,
        Error
    }

    public interface ICustomClientFunctionsProvider
    {
        Task<EmailConfirmationStatus> HasConfirmedEmailAsync();
        CardBrandDimensions GetBrandDimensions(string brand);
        bool CardTypeExists(string cardType);
        bool CardBrandExists(string cardBrand);
        Task<List<SelectListItem>> GetSupportedCryptocurrenciesAsync();
        Task<SelectListItem> GetSupportedCryptocurrencyAsync(string validatorId, string coinId);
        Task<List<SelectListItem>> GetNetworksForCryptocurrencyAsync(string coinId);
        List<SelectListItem> GetSupportedCountries();
    }
}
