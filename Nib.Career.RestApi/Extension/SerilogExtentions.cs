using System;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SumoLogic;

namespace Nib.Career.RestApi.Extensions
{
    /// <summary>
    /// Extensions for setting up serilog using defaults across the Nib project.
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Setup a default logger.
        /// </summary>
        public static LoggerConfiguration WithDefaults(this LoggerConfiguration config)
        {
            config
                .WithMinimumLevel()
                .WithMinimumLevelOverrides()
                .Enrich.FromLogContext()
                .WriteToConsole();

            var url = Environment.GetEnvironmentVariable("SUMOLOGIC_RECEIVER_URL");
            if (!string.IsNullOrEmpty(url))
            {
                config.WriteToSumologic(url);
            }

            return config;
        }

        /// <summary>
        /// Setup minimum levels for debug and release builds.
        /// </summary>
        public static LoggerConfiguration WithMinimumLevel(this LoggerConfiguration config)
        {
#if DEBUG
            config.MinimumLevel.Debug();
#else
            config.MinimumLevel.Information();
#endif
            return config;
        }

        /// <summary>
        /// Setup minium level overrides.
        /// </summary>
        public static LoggerConfiguration WithMinimumLevelOverrides(this LoggerConfiguration config)
        {
            config
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information);
            return config;
        }

        /// <summary>
        /// Write to console using defaults.
        /// </summary>
        public static LoggerConfiguration WriteToConsole(this LoggerConfiguration config, string outputTemplate = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        {
            config.WriteTo.Console(outputTemplate: outputTemplate);
            return config;
        }

        /// <summary>
        /// Write to sumologic sink using defaults.
        /// </summary>
        public static LoggerConfiguration WriteToSumologic(this LoggerConfiguration config, string url)
        {
            config
                .Enrich.WithProperty("k8sPodNamespace", Environment.GetEnvironmentVariable("K8S_POD_NAMESPACE"))
                .Enrich.WithProperty("k8sPodName", Environment.GetEnvironmentVariable("K8S_POD_NAME"))
                .Enrich.WithProperty("k8sPodIp", Environment.GetEnvironmentVariable("K8S_POD_IP"))
                .WriteTo.SumoLogic(url, Environment.GetEnvironmentVariable("K8S_NODE_NAME"), textFormatter: new RenderedCompactJsonFormatter());
            return config;
        }
    }
}