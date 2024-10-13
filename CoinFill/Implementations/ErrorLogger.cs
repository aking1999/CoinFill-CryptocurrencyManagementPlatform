using CoinFill.Helpers;
using CoinFill.Helpers.Extensions;
using CoinFill.Interfaces;
using CoinFill.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoinFill.Implementations
{
    public class ErrorLogger : IErrorLogger
    {
        private ClaimsPrincipal User;
        private readonly CoinFillContext _context;

        public ErrorLogger(IHttpContextAccessor contextAccessor)
        {
            _context = new CoinFillContext();

            try
            {
                User = contextAccessor.HttpContext.User;
            }
            catch (Exception)
            {
                User = null;
            }
        }

        public void SaveError(string errorMessage, string area, string controller, string action)
        {
            throw new NotImplementedException();
        }

        public void SaveError(Exception e, string area, string controller, string action)
        {
            try
            {
                var userId = User != null ? "Anonymous" : "HttpContext.User is NULL";

                if (User != null && User.Identity.IsAuthenticated)
                    userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                _context.ErrorLogs.Add(new ErrorLogs
                {
                    Id = Helper.GenerateNumbersId(),
                    UserIdOrAnonymous = userId,
                    AreaOrProject = area?.TakeMax(64),
                    ControllerOrClass = controller?.TakeMax(64),
                    ActionOrMethod = action?.TakeMax(64),
                    Description = e.GetRootException()?.Message?.TakeMax(1024),
                    Source = e.Source?.TakeMax(512),
                    StackTrace = e.StackTrace?.TakeMax(4096),
                    StackTraceFrameMethodName = new StackTrace(e).GetFrame(0)?.GetMethod()?.Name?.TakeMax(512),
                    StackTraceExecutingAssemblyName = new StackTrace(e).GetFrames()?
                                                                       .Select(f => f?.GetMethod())?
                                                                       .FirstOrDefault(m => m?.Module?.Assembly == Assembly.GetExecutingAssembly())?
                                                                       .Name?
                                                                       .TakeMax(512),
                    TargetSiteName = e.TargetSite?.Name?.TakeMax(512),
                    TargetSiteReflectedTypeFullName = e.TargetSite?.ReflectedType?.FullName?.TakeMax(512),
                    ErrorDateTime = DateTime.UtcNow,
                    Fixed = false
                });
            }
            catch (Exception ex)
            {
                _context.ErrorLogs.Add(new ErrorLogs
                {
                    Id = Helper.GenerateNumbersId(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = "CoinFill",
                    ControllerOrClass = "ErrorLogger",
                    ActionOrMethod = "SaveError",
                    Description = ex.GetRootException()?.Message?.TakeMax(1024),
                    Source = ex.Source?.TakeMax(512),
                    StackTrace = e.StackTrace?.TakeMax(4096),
                    StackTraceFrameMethodName = new StackTrace(ex).GetFrame(0)?.GetMethod()?.Name?.TakeMax(512),
                    StackTraceExecutingAssemblyName = new StackTrace(ex).GetFrames()?
                                                                        .Select(f => f?.GetMethod())?
                                                                        .FirstOrDefault(m => m?.Module?.Assembly == Assembly.GetExecutingAssembly())?
                                                                        .Name?
                                                                        .TakeMax(512),
                    TargetSiteName = e.TargetSite?.Name?.TakeMax(512),
                    TargetSiteReflectedTypeFullName = e.TargetSite?.ReflectedType?.FullName?.TakeMax(512),
                    ErrorDateTime = DateTime.UtcNow,
                    Fixed = false
                });
            }
            finally
            {
                _context.SaveChanges();
            }
        }

        public async Task SaveErrorAsync(string errorMessage, string area, string controller, string action)
        {
            try
            {
                var userId = User != null ? "Anonymous" : "HttpContext.User is NULL";

                if (User != null && User.Identity.IsAuthenticated)
                    userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _context.ErrorLogs.AddAsync(new ErrorLogs
                {
                    Id = Helper.GenerateNumbersId(),
                    UserIdOrAnonymous = userId,
                    AreaOrProject = area?.TakeMax(64),
                    ControllerOrClass = controller?.TakeMax(64),
                    ActionOrMethod = action?.TakeMax(64),
                    Description = errorMessage?.TakeMax(1024),
                    ErrorDateTime = DateTime.UtcNow,
                    Fixed = false
                });
            }
            catch (Exception e)
            {
                await _context.ErrorLogs.AddAsync(new ErrorLogs
                {
                    Id = Helper.GenerateNumbersId(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = "CoinFill",
                    ControllerOrClass = "ErrorLogger",
                    ActionOrMethod = "SaveErrorAsync",
                    Description = e.GetRootException()?.Message?.TakeMax(1024),
                    Source = e.Source?.TakeMax(512),
                    StackTrace = e.StackTrace?.TakeMax(4096),
                    StackTraceFrameMethodName = new StackTrace(e).GetFrame(0)?.GetMethod()?.Name?.TakeMax(512),
                    StackTraceExecutingAssemblyName = new StackTrace(e).GetFrames()?
                                                                       .Select(f => f?.GetMethod())?
                                                                       .FirstOrDefault(m => m?.Module?.Assembly == Assembly.GetExecutingAssembly())?
                                                                       .Name?
                                                                       .TakeMax(512),
                    TargetSiteName = e.TargetSite?.Name?.TakeMax(512),
                    TargetSiteReflectedTypeFullName = e.TargetSite?.ReflectedType?.FullName?.TakeMax(512),
                    ErrorDateTime = DateTime.UtcNow,
                    Fixed = false
                });
            }
            finally
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveErrorAsync(Exception e, string area, string controller, string action)
        {
            try
            {
                var userId = User != null ? "Anonymous" : "HttpContext.User is NULL";

                if (User != null && User.Identity.IsAuthenticated)
                    userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _context.ErrorLogs.AddAsync(new ErrorLogs
                {
                    Id = Helper.GenerateNumbersId(),
                    UserIdOrAnonymous = userId,
                    AreaOrProject = area?.TakeMax(64),
                    ControllerOrClass = controller?.TakeMax(64),
                    ActionOrMethod = action?.TakeMax(64),
                    Description = e.GetRootException()?.Message?.TakeMax(1024),
                    Source = e.Source?.TakeMax(512),
                    StackTrace = e.StackTrace?.TakeMax(4096),
                    StackTraceFrameMethodName = new StackTrace(e).GetFrame(0)?.GetMethod()?.Name?.TakeMax(512),
                    StackTraceExecutingAssemblyName = new StackTrace(e).GetFrames()?
                                                                       .Select(f => f?.GetMethod())?
                                                                       .FirstOrDefault(m => m?.Module?.Assembly == Assembly.GetExecutingAssembly())?
                                                                       .Name?
                                                                       .TakeMax(512),
                    TargetSiteName = e.TargetSite?.Name?.TakeMax(512),
                    TargetSiteReflectedTypeFullName = e.TargetSite?.ReflectedType?.FullName?.TakeMax(512),
                    ErrorDateTime = DateTime.UtcNow,
                    Fixed = false
                });
            }
            catch (Exception ex)
            {
                await _context.ErrorLogs.AddAsync(new ErrorLogs
                {
                    Id = Helper.GenerateNumbersId(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = "CoinFill",
                    ControllerOrClass = "ErrorLogger",
                    ActionOrMethod = "SaveErrorAsync",
                    Description = ex.GetRootException()?.Message?.TakeMax(1024),
                    Source = ex.Source?.TakeMax(512),
                    StackTrace = e.StackTrace?.TakeMax(4096),
                    StackTraceFrameMethodName = new StackTrace(ex).GetFrame(0)?.GetMethod()?.Name?.TakeMax(512),
                    StackTraceExecutingAssemblyName = new StackTrace(ex).GetFrames()?
                                                                        .Select(f => f?.GetMethod())?
                                                                        .FirstOrDefault(m => m?.Module?.Assembly == Assembly.GetExecutingAssembly())?
                                                                        .Name?
                                                                        .TakeMax(512),
                    TargetSiteName = e.TargetSite?.Name?.TakeMax(512),
                    TargetSiteReflectedTypeFullName = e.TargetSite?.ReflectedType?.FullName?.TakeMax(512),
                    ErrorDateTime = DateTime.UtcNow,
                    Fixed = false
                });
            }
            finally
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
