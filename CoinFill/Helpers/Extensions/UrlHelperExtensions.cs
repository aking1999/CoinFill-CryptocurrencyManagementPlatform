using CoinFill.Helpers.Models;
using CoinFill.Interfaces;
using CoinFill.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoinFill.Helpers.Extensions
{
    public static class UrlHelperExtensions
    {
        private static IErrorLogger _errors => new HttpContextAccessor().HttpContext?.RequestServices?.GetRequiredService<IErrorLogger>();
        private static ClaimsPrincipal User => new HttpContextAccessor().HttpContext?.User;

        public static async Task<string> GetNotificationsUrlBasedOnRole(this IUrlHelper url)
        {
            try
            {
                var userRole = await User.GetRoleAsync();

                if (userRole == default) throw new GeneralException("User does not have a role or has multiple.", signOutUser: true);

                if (userRole.ToLower() == UserRoles.Admin.ToLower())
                    return "/admin/notifications/";

                else
                    return "/notifications/";
            }
            catch (Exception e)
            {
                await _errors.SaveErrorAsync(e, "CoinFill", "UrlHelperExtensions", "GetNotificationsUrlBasedOnRole");
                return string.Empty;
            }
        }
    }
}
