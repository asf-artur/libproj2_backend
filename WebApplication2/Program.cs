using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Conditions;
using NLog.Targets;
using NLog.Web;

namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new NLog.Config.LoggingConfiguration();
            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"Logs\\logs_{DateTime.Now}.txt" };
            var logconsole = new NLog.Targets.ColoredConsoleTarget("logconsole");

            var criticalHighlightRule = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level > LogLevel.Error"),
                ForegroundColor = ConsoleOutputColor.White,
                BackgroundColor = ConsoleOutputColor.DarkRed
            };

            logconsole.RowHighlightingRules.Add(criticalHighlightRule);
            logconsole.UseDefaultRowHighlightingRules = true;

            // Rules for mapping loggers to targets
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);

            var logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}