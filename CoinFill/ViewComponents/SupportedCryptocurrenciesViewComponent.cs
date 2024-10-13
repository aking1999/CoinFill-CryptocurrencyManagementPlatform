using CoinFill.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewComponents
{
    public class SupportedCryptocurrenciesViewComponent : ViewComponent
    {
        private readonly CoinFillContext _context;

        public SupportedCryptocurrenciesViewComponent()
        {
            _context = new CoinFillContext();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Cryptocurrencies.AsNoTracking().OrderBy(c => c.OrderNumber).ToListAsync());
        }
    }
}
