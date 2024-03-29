using System;
using GhostNetwork.Profiles.Api.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;

namespace GhostNetwork.Profiles.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.With<UtcTimestampEnricher>()
                .Filter.ByIncludingOnly(Matching.FromSource("GhostNetwork"))
                .WriteTo.Console(outputTemplate: "{UtcTimestamp:yyyy-MM-ddTHH:mm:ss.ffffZ} [{Level:u3}] {Message:l} {Properties:j}{NewLine}{Exception}")
                .CreateLogger();

            var startupLogger = Log.ForContext<Program>();

            try
            {
                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureLogging(o =>
                    {
                        o.Configure(c => c.ActivityTrackingOptions = ActivityTrackingOptions.None);
                    })
                    .UseSerilog()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                    .Build();

                host.Start();
                startupLogger.Information("Starting http server on port 5010");

                host.WaitForShutdown();
                return 0;
            }
            catch (Exception ex)
            {
                startupLogger.Error(ex.Message);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
