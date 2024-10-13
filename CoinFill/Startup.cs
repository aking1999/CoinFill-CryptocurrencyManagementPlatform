using CoinFill.Data;
using CoinFill.Emails;
using CoinFill.Helpers.ActionFilters;
using CoinFill.Helpers.RouteMiddlewares;
using CoinFill.Implementations;
using CoinFill.Interfaces;
using CoinFill.Models;
using CoinFill.Notifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Winton.AspNetCore.Seo;
using Winton.AspNetCore.Seo.Robots;

namespace CoinFill
{
    public class Startup
    {
        public Startup(IWebHostEnvironment _environment)
        {
            Configuration = new ConfigurationBuilder()
                                .SetBasePath(_environment.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{_environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                                .Build();

            Environment = _environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            var appName = Configuration.GetSection("Application:AppName")?.Value;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(1000000)));

            services.AddIdentity<CustomClient, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = false;
            });

            services.AddSeoWithDefaultRobots();

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.GetFullPath(Path.Combine(Environment.ContentRootPath, @"DataProtection\"))));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = $"{appName}.Authentication";
                options.LoginPath = new PathString("/auth/signin");
                options.AccessDeniedPath = new PathString("/error/access-denied");
            });

            services.Configure<CookieTempDataProviderOptions>(options => options.Cookie.Name = $"{appName}.Temporary");

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(UserActivityLoggerAttribute));
            });

            services.AddRazorPages();

            services.AddSession(options =>
            {
                options.Cookie.Name = $"{appName}.Session";
            });

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped(x =>
                    x.GetRequiredService<IUrlHelperFactory>()
                        .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            services.AddScoped<IPasswordHasher<CustomClient>, PasswordHasher<CustomClient>>();

            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddSignalR();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddTransient<IErrorLogger, ErrorLogger>();
            services.AddTransient<IMailService, MailService>();

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{appName}.AntiForgery";
            });

            services.AddSeo(
                options =>
                {
                    options.Sitemap.Urls = Helpers.SeoHelper.SitemapUrls;
                    options.RobotsTxt.UserAgentRecords = new List<UserAgentRecord>
                    {
                        new UserAgentRecord
                        {
                            Disallow = Helpers.SeoHelper.DisabledUrls.Concat(Helpers.SeoHelper.DisabledContent),
                            NoIndex = Helpers.SeoHelper.DisabledUrls
                        }
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseExceptionHandler("/error/error-response");
            }

            var rewriteOptions = new RewriteOptions();
            rewriteOptions.AddRedirectToHttpsPermanent();
            app.UseRewriter(rewriteOptions);
            app.UseStaticFiles();

            rewriteOptions.Add(new RedirectToNonWwwRouteRule(new SystemErrorLogger()));
            rewriteOptions.AddRedirect("(.*)/$", "$1", 301);
            app.UseRewriter(rewriteOptions);

            app.UseMiddleware<IgnoreRouteMiddleware>();
            app.UseMiddleware<EmailCampaignRouteMiddleware>();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>($"/{NotificationHub.Url}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
