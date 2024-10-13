using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Winton.AspNetCore.Seo.Sitemaps;

namespace CoinFill.Helpers
{
    public class SeoHelper
    {
        private static IUrlHelper _url => new HttpContextAccessor().HttpContext.RequestServices.GetRequiredService<IUrlHelper>();
        public static List<SitemapUrlOptions> SitemapUrls
        {
            get
            {
                var sitemapUrls = new List<SitemapUrlOptions>
                {
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("Index", "Home"),
                        Priority = 1M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("HighApyStaking", "Staking"),
                        Priority = 1M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("ExclusiveRewards", "Home"),
                        Priority = 1M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("Dashboard", "Demo"),
                        Priority = 1M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("Staking", "Demo"),
                        Priority = 1M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("Cards", "Demo"),
                        Priority = 1M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("BankAccounts", "Demo"),
                        Priority = 0.9M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("MyStakes", "Demo"),
                        Priority = 0.9M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("TransferToPayPal", "Demo"),
                        Priority = 0.8M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("ContactUs", "Home"),
                        Priority = 0.7M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("CustomerSupport", "Home"),
                        Priority = 0.7M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("AboutUs", "Home"),
                        Priority = 0.7M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("SignIn", "Authorization"),
                        Priority = 0.6M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("Register", "Authorization"),
                        Priority = 0.6M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("Terms", "Home"),
                        Priority = 0.6M
                    },
                    new SitemapUrlOptions
                    {
                        RelativeUrl = _url.Action("Privacy", "Home"),
                        Priority = 0.6M
                    }
                };

                return sitemapUrls;
            }
        }

        public static List<string> DisabledUrls { get; } = new List<string>
        {

            "/base/",
            "/rpc/",
            "/subscribe-to-newsletter",
            //ajax urls
            _url.Action("LogFrontEndErrors", "Error"),
        };

        public static List<string> DisabledContent { get; } = new List<string>
        {
            _url.Content("~/images/"),
            _url.Content("~/generated/")
        };
    }
}
