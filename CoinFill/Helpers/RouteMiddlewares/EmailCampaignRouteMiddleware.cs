using CoinFill.Helpers.Extensions;
using CoinFill.Implementations;
using CoinFill.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Helpers.RouteMiddlewares
{
    public class EmailCampaignRouteMiddleware
    {
        private readonly ISystemErrorLogger _systemError;
        private readonly RequestDelegate _next;
        private readonly CoinFill.Models.CoinFillContext _context;

        public EmailCampaignRouteMiddleware(RequestDelegate next)
        {
            _systemError = new SystemErrorLogger();
            _next = next;
            _context = new CoinFill.Models.CoinFillContext();
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.Path.HasValue &&
                    context.Request.Query.ContainsKey("campaign") &&
                    context.Request.Query.ContainsKey("usource"))
                {
                    bool unsub = false;
                    if (context.Request.Query.ContainsKey("unsubscribe") &&
                        context.Request.Query["unsubscribe"] == "true") unsub = true;

                    var cmp = "launch_celebration";
                    var eml = context.Request.Query["usource"].ToString();
                    var uc = await _context.EmailMarketingStatistics.SingleOrDefaultAsync(c => c.Email == eml && c.CampaignName == cmp);

                    if (uc != default)
                    {
                        uc.OpenCount++;
                        uc.Unsubscribed = unsub;
                        uc.IpAddress = context.Connection?.RemoteIpAddress?.ToString().TakeMax(256);
                        uc.LastOpenedDateTime = DateTime.UtcNow;
                        _context.EmailMarketingStatistics.Update(uc);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        await _context.EmailMarketingStatistics.AddAsync(new CoinFill.Models.EmailMarketingStatistics
                        {
                            Id = "coinfill-campaign-" + Helper.GenerateNumbersId(),
                            CampaignName = cmp,
                            Email = eml,
                            OpenCount = 1,
                            Unsubscribed = unsub,
                            IpAddress = context.Connection?.RemoteIpAddress?.ToString().TakeMax(256),
                            LastOpenedDateTime = DateTime.UtcNow
                        });
                        await _context.SaveChangesAsync();
                    }
                }

                await _next.Invoke(context);
            }
            catch(Exception e)
            {
                await _systemError.SaveErrorAsync(e, "CoinFill", "EmailCampaignRouteMiddleware", "Invoke");
                await _next.Invoke(context);
            }
        }
    }
}
