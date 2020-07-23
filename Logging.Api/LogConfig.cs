using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using Serilog.Core;
using Logging.Api.Services;

namespace Logging.Api
{
    public static class LogConfig
    {        
        public static void ConfigureCentralLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();

            CustomLogLevels.GetLogLevel.MinimumLevel = Serilog.Events.LogEventLevel.Warning;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.ControlledBy(CustomLogLevels.GetLogLevel) // can be updated dynamically
                .Enrich.WithProperty("Environment", environment)
                //.WriteTo.File(@"D:\Log123.txt")
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .CreateLogger();
        }
               
        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                //IndexFormat - > txloggingapi-development-2020-05                
                //FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                //EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                //                       EmitEventFailureHandling.WriteToFailureSink |
                //                       EmitEventFailureHandling.RaiseCallback,
                //FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null),
                //BufferCleanPayload = (failingEvent, statuscode, exception) =>
                //{
                //    dynamic e = JObject.Parse(failingEvent);
                //    return JsonConvert.SerializeObject(new Dictionary<string, object>()
                //        {
                //            { "@timestamp",e["@timestamp"]},
                //            { "level","Error"},
                //            { "message","Error: "+e.message},
                //            { "messageTemplate",e.messageTemplate},
                //            { "failingStatusCode", statuscode},
                //            { "failingException", exception}
                //        });
                //},
                //BufferIndexDecider = (logEvent, offset) => "log-serilog-" + (new Random().Next(0, 2))
            };
        }
    }
}
