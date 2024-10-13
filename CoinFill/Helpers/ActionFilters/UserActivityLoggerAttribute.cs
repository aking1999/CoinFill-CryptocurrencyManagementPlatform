using CoinFill.Helpers.Extensions;
using CoinFill.Interfaces;
using CoinFill.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using UAParser;

namespace CoinFill.Helpers.ActionFilters
{
    public class UserActivityLoggerAttribute : ActionFilterAttribute
    {
        private readonly IErrorLogger _errors;
        private readonly CoinFillContext _context;

        public UserActivityLoggerAttribute(IErrorLogger errors)
        {
            _errors = errors;
            _context = new CoinFillContext();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                context.HttpContext.Items["actionArguments"] = !string.IsNullOrWhiteSpace(context.HttpContext.Request?.QueryString.Value) ?
                                context.HttpContext.Request?.QueryString.Value :
                                JsonConvert.SerializeObject(context.ActionArguments);
            }
            catch (Exception e)
            {
                _errors.SaveError(e, "CoinFill", "UserActivityLoggerAttribute", "OnActionExecuting");
            }
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            try
            {
                var controller = context.HttpContext.GetRouteValue("controller")?.ToString();
                var action = context.HttpContext.GetRouteValue("action")?.ToString();
                var userAgentParser = Parser.GetDefault().Parse(context.HttpContext.Request.Headers["User-Agent"]);

                var activityLog = new UserActivityLogs
                {
                    Id = Helper.GenerateNumbersId(),
                    UserIdOrAnonymous = context.HttpContext.User.Identity.IsAuthenticated ? context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.TakeMax(450) : "Anonymous",
                    Area = null,
                    Controller = controller.TakeMax(64),
                    Action = action.TakeMax(64),
                    QueryDataJson = context.HttpContext.Items["actionArguments"].ToString().TakeMax(2048),
                    MethodType = context.HttpContext.Request.Method,
                    UserAgent = userAgentParser.ToString().TakeMax(1024),
                    IsCrawler = userAgentParser.Device.IsSpider,
                    IpAddress = context.HttpContext?.Connection?.RemoteIpAddress?.ToString().TakeMax(256),
                    ActivityDateTime = DateTime.UtcNow
                };

                _context.UserActivityLogs.Add(activityLog);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _errors.SaveError(e, "CoinFill", "UserActivityLoggerAttribute", "OnResultExecuted");
            }
        }
    }
}
