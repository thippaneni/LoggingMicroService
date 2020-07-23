using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Logging.Api.Models;
using Logging.Api.Models.Enums;
using Serilog.Events;

namespace Logging.Api.Services.Extentions
{
    public static class LoggerExtensions
    {       
        public static void Emit<T>(this ILogger<T> @this, LogsForCustomer custEventId, 
            ApiLogLevel apiLogLevel, Exception exception,
        string message, params object[] args)
        {
            var i = (int)Convert.ChangeType(custEventId, custEventId.GetTypeCode());
            var eventName = Enum.GetName(custEventId.GetType(), custEventId);
            var eventScope = custEventId.GetType().Name;
            SetCustomerLogLevel(custEventId, apiLogLevel);
            var parameters = args.Append(eventScope).Append(eventName).ToArray();            
            var lvl = GetCustomerLogLevel_1(custEventId, apiLogLevel);
            @this.Log(lvl, i, exception, message + " (Event scope: {eventScope}, name: {eventName})", parameters);
        }
        
        private static Dictionary<Enum, ApiLogLevel> GetCustomerLogLevels()
        {
            var dict = new Dictionary<Enum, ApiLogLevel>();
            string Json = System.IO.File.ReadAllText(@"D:\POC\ABCTech-Eco-System\CentralLogging\Logging.Api.Services\CustomerLogLevels\custumerloglevels.json");
            var list = JsonSerializer.Deserialize<List<CustomerLogLevel>>(Json);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    dict.Add(list[i].CustomerId, list[i].LogLevel);
                }
            }
            
            return dict;
        }

        private static void SetCustomerLogLevel(LogsForCustomer customerId, ApiLogLevel defaultLogLevel)
        {
            var logLevel = defaultLogLevel;
            var custLogLevel = GetCustLogeLevelsFromJson().Where(cust => cust.CustomerId == customerId).SingleOrDefault();
            if (custLogLevel != null)
            {
                logLevel = custLogLevel.LogLevel;
                CustomLogLevels.GetLogLevel.MinimumLevel = FromApiLogLevelToLogEventLevel(logLevel);
            }
        }

        private static LogLevel GetCustomerLogLevel_1(LogsForCustomer customerId, ApiLogLevel defaultLogLevel)
        {
            var logLevel = defaultLogLevel;
            var custLogLevel = GetCustLogeLevelsFromJson().Where(cust => cust.CustomerId == customerId).SingleOrDefault();
            if (custLogLevel != null)
            {
                logLevel = custLogLevel.LogLevel;
            }

            return FromAPILogLevelToLogLevel(logLevel);
        }

        private static List<CustomerLogLevel> GetCustLogeLevelsFromJson()
        {
            string Json = System.IO.File.ReadAllText(@"D:\POC\ABCTech-Eco-System\CentralLogging\Logging.Api.Services\CustomerLogLevels\custumerloglevels.json");
            var list = JsonSerializer.Deserialize<List<CustomerLogLevel>>(Json);
            if (list != null && list.Count > 0)
            {
                return list;
            }
            return new List<CustomerLogLevel>(); ;
        }
        private static LogLevel FromAPILogLevelToLogLevel(ApiLogLevel apiLogLevel)
        {
            switch (apiLogLevel)
            {
                case ApiLogLevel.Trace:
                    return LogLevel.Trace;
                case ApiLogLevel.Debug:
                    return LogLevel.Debug;
                case ApiLogLevel.Information:
                    return LogLevel.Information;
                case ApiLogLevel.Warning:
                    return LogLevel.Warning;
                case ApiLogLevel.Error:
                    return LogLevel.Error;
                case ApiLogLevel.Critical:
                    return LogLevel.Critical; ;
                case ApiLogLevel.None:
                    return LogLevel.None;
                default:
                    return LogLevel.None;
            }
        }
        private static LogEventLevel FromApiLogLevelToLogEventLevel(ApiLogLevel apiLogLevel)
        {
            switch (apiLogLevel)
            {
                case ApiLogLevel.Trace:
                    return LogEventLevel.Verbose;
                case ApiLogLevel.Debug:
                    return LogEventLevel.Debug;
                case ApiLogLevel.Information:
                    return LogEventLevel.Information;
                case ApiLogLevel.Warning:
                    return LogEventLevel.Warning;
                case ApiLogLevel.Error:
                    return LogEventLevel.Error;
                case ApiLogLevel.Critical:
                    return LogEventLevel.Fatal;                
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
