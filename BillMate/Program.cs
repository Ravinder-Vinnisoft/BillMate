using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace BillMate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Setup().LoadConfigurationFromAppSettings();
            var logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                logger.Info("Initializing application...");
                CreateHostBuilder(args).UseDefaultServiceProvider(options =>
                        options.ValidateScopes = false).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped because of exception.");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            };
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseNLog(); // Add this line to use NLog

                });
    }
}
