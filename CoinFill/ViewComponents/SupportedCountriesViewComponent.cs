using CoinFill.Helpers.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoinFill.ViewComponents
{
    public class SupportedCountriesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(SupportedCountriesProvider.SupportedCountries);
        }
    }
}
