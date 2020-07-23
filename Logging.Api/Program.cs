using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Logging.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogConfig.ConfigureCentralLogging();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration(configuration =>
                //{
                //    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                //    configuration.AddJsonFile(
                //        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                //        optional: true);
                //})
                //.ConfigureLogging((builderContext, logging) =>
                //{
                //    // Requires `using Microsoft.Extensions.Logging;`
                //    logging.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                //    logging.ClearProviders();
                //    logging.AddConsole();
                //    logging.AddDebug();
                //    logging.AddEventSourceLogger();                    
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
