using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging.Api.Services
{
    public static class CustomLogLevels
    {
        static CustomLogLevels()
        {
            GetLogLevel = new LoggingLevelSwitch();
        }
        public static LoggingLevelSwitch GetLogLevel { get; set; }
    }
}
