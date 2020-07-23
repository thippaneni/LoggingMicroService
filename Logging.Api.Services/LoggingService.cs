using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Logging.Api.Models;
using Logging.Api.Models.Enums;
using Logging.Api.Services.Extentions;

namespace Logging.Api.Services
{    
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;               
        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }
        public async Task<bool> HealthAsync()
        {
            string message = string.Empty;
            var client = new Nest.ElasticClient(new Uri("http://localhost:9200"));
            var response = await client.Cluster.HealthAsync();

            if (response.ApiCall.HttpStatusCode == 200 && response.ApiCall.Success)
            {
                var healthColor = response.Status.ToString().ToLower();
                if (healthColor == "green" || healthColor == "yellow")
                {
                    return true;
                }
            }

            return false;
        }

        public Task<LogResponseModel> LogAsync(LoggingModel logModel)
        {
            var appInfo = logModel.AppInfo == null ? new ApplicationInfo() : logModel.AppInfo;
            var custId = LogsForCustomer.None;
            var custLogLevel = ApiLogLevel.Information;

            if (logModel.CustomerLogLevel != null)
            {
                custId = logModel.CustomerLogLevel.CustomerId;
                custLogLevel = logModel.CustomerLogLevel.LogLevel;
            }
            string message = $"\nLog raised with Id - {logModel.Id} at {logModel.LogRaisedAt}\n" +
                    $"from Service - {appInfo.ServiceName} And Application - {appInfo.AppName}\n" +
                    $"IP_Adress {appInfo.IPAdress} with Message {logModel.Message}\n" +
                    $"for Customer: {custId}\n" + 
                    $"and Logging to ES at {DateTime.UtcNow}";
            
            _logger.Emit(custId, custLogLevel, logModel.Exception, message, logModel);         
            
            
            var response = new LogResponseModel()
            {
                Id = logModel.Id,
                Status = LogStatus.Completed
            };
            return Task.FromResult(response);
        }
    }
}
