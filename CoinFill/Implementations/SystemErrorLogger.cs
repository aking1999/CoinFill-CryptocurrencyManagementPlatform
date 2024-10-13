using CoinFill.Helpers.Extensions;
using CoinFill.Interfaces;
using CoinFill.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoinFill.Implementations
{
    public class SystemErrorLogger : ISystemErrorLogger
    {
        private readonly CoinFillContext _context;

        public SystemErrorLogger()
        {
            _context = new CoinFillContext();
        }

        public void SaveError(string errorMessage, string project, string @class, string method)
        {
            throw new NotImplementedException();
        }

        public void SaveError(Exception e, string project, string @class, string method)
        {
            try
            {
                _context.ErrorLogs.Add(new ErrorLogs
                {
                    Id = Guid.NewGuid().ToString(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = project?.TakeMax(64),
                    ControllerOrClass = @class?.TakeMax(64),
                    ActionOrMethod = method?.TakeMax(64),
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
                    Id = Guid.NewGuid().ToString(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = "CoinFill",
                    ControllerOrClass = "SystemErrorLogger",
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

        public async Task SaveErrorAsync(string errorMessage, string project, string @class, string method)
        {
            try
            {
                await _context.ErrorLogs.AddAsync(new ErrorLogs
                {
                    Id = Guid.NewGuid().ToString(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = project?.TakeMax(64),
                    ControllerOrClass = @class?.TakeMax(64),
                    ActionOrMethod = method?.TakeMax(64),
                    Description = errorMessage?.TakeMax(1024),
                    ErrorDateTime = DateTime.UtcNow,
                    Fixed = false
                });
            }
            catch (Exception e)
            {
                await _context.ErrorLogs.AddAsync(new ErrorLogs
                {
                    Id = Guid.NewGuid().ToString(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = "CoinFill",
                    ControllerOrClass = "SystemErrorLogger",
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

        public async Task SaveErrorAsync(Exception e, string project, string @class, string method)
        {
            try
            {
                await _context.ErrorLogs.AddAsync(new ErrorLogs
                {
                    Id = Guid.NewGuid().ToString(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = project?.TakeMax(64),
                    ControllerOrClass = @class?.TakeMax(64),
                    ActionOrMethod = method?.TakeMax(64),
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
                    Id = Guid.NewGuid().ToString(),
                    UserIdOrAnonymous = "System",
                    AreaOrProject = "Framework",
                    ControllerOrClass = "SystemErrorLogger",
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
