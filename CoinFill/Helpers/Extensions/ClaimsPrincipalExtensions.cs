using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinFill.Models;
using Microsoft.AspNetCore.Hosting;
using CoinFill.Interfaces;
using CoinFill.Implementations;

namespace CoinFill.Helpers.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        private static IFileRepository _files;

        private static UserManager<CustomClient> _userManager => new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<UserManager<CustomClient>>();

        static ClaimsPrincipalExtensions()
        {
            _files = new FileRepository();
        }

        public static string GetProfilePhotoPathOrDefaultPhotoPath(this ClaimsPrincipal User, IWebHostEnvironment environment)
        {
            return _files.GetUserProfilePhotoPathOrDefaultPhotoPath(_userManager, environment, User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static async Task<string> GetRoleAsync(this ClaimsPrincipal User)
        {
            if (!User.Identity.IsAuthenticated) return null;

            var user = await _userManager.GetUserAsync(User);

            if (user == null) return null;

            return (await _userManager.GetRolesAsync(user))?.SingleOrDefault();
        }
    }
}