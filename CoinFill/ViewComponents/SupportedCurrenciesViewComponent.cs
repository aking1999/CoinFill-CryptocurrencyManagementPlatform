using CoinFill.Helpers.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewComponents
{
    public class SupportedCurrenciesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(SupportedCurrenciesProvider.SupportedCurrencies);
        }
    }
}
