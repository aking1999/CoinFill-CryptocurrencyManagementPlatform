using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class ErrorLogs
    {
        public string Id { get; set; }
        public string UserIdOrAnonymous { get; set; }
        public string AreaOrProject { get; set; }
        public string ControllerOrClass { get; set; }
        public string ActionOrMethod { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string StackTraceFrameMethodName { get; set; }
        public string StackTraceExecutingAssemblyName { get; set; }
        public string TargetSiteName { get; set; }
        public string TargetSiteReflectedTypeFullName { get; set; }
        public string StackTrace { get; set; }
        public DateTime? ErrorDateTime { get; set; }
        public bool? Fixed { get; set; }
    }
}
