using System;
using Logging.Api.Models.Enums;

namespace Logging.Api.Models
{
    public class LoggingModel
    {
        public string Id { get; set; }
        public ApplicationInfo AppInfo { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public DateTimeOffset LogRaisedAt { get; set; }
        public CustomerLogLevel CustomerLogLevel { get; set; }

    }
}
