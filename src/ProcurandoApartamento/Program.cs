// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using JHipsterNet.Web.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Syslog;
using System;
using System.IO;
using System.Security.Authentication;
using static JHipsterNet.Core.Boot.BannerPrinter;
using ILogger = Serilog.ILogger;

namespace ProcurandoApartamento
{
    public class Program
    {
        const string SerilogSection = "Serilog";
        const string SyslogPort = "SyslogPort";
        const string SyslogUrl = "SyslogUrl";
        const string SyslogAppName = "SyslogAppName";

        public static int Main(string[] args)
        {
            PrintBanner(10 * 1000);

            try
            {
                IConfiguration appConfiguration = GetAppConfiguration();
                Log.Logger = CreateLogger(appConfiguration);

                CreateHostBuilder(args)
                    .Build()
                    .Run();

                return 0;
            }
            catch (Exception ex)
            {
                // Use ForContext to give a context to this static environment (for Serilog LoggerNameEnricher).
                Log.ForContext<Program>().Fatal(ex, $"Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog();

        /// <summary>
        /// Create application logger from configuration.
        /// </summary>
        /// <returns></returns>
        private static ILogger CreateLogger(IConfiguration appConfiguration)
        {
            var port = 6514;

            // for logger configuration
            // https://github.com/serilog/serilog-settings-configuration
            if (appConfiguration.GetSection(SerilogSection)[SyslogPort] != null)
            {
                if (int.TryParse(appConfiguration.GetSection(SerilogSection)[SyslogPort], out var portFromConf))
                {
                    port = portFromConf;
                }
            }

            var url = appConfiguration.GetSection(SerilogSection)[SyslogUrl] != null
                ? appConfiguration.GetSection(SerilogSection)[SyslogUrl]
                : "localhost";
            var appName = appConfiguration.GetSection(SerilogSection)[SyslogAppName] != null
                ? appConfiguration.GetSection(SerilogSection)[SyslogAppName]
                : "ProcurandoApartamentoApp";
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.With<LoggerNameEnricher>()
                .WriteTo.TcpSyslog(url, port, appName, FramingType.OCTET_COUNTING, SyslogFormat.RFC5424, Facility.Local0, SslProtocols.None)
                .ReadFrom.Configuration(appConfiguration);

            return loggerConfiguration.CreateLogger();
        }

        /// <summary>
        /// Gets the current application configuration
        /// from global and specific appsettings.
        /// </summary>
        /// <returns>Return the application <see cref="IConfiguration"/></returns>
        private static IConfiguration GetAppConfiguration()
        {
            // Actually, before ASP.NET bootstrap, we must rely on environment variable to get environment name
            // https://docs.microsoft.com/fr-fr/aspnet/core/fundamentals/environments?view=aspnetcore-2.2
            // Pay attention to casing for Linux environment. By default it's pascal case.
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
