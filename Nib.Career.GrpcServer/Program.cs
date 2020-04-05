using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Nib.Career.Core.Extensions;
using Serilog;
using System;

namespace Nib.Career.GrpcServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WithDefaults().CreateLogger();
            try
            {
                Log.Logger.ForContext<Program>().Information("Starting");
                var host = CreateHostBuilder(args).Build();
                host.Run();
                Log.Logger.ForContext<Program>().Information("Stopping");
            }
            catch (Exception e)
            {
                Log.Logger.ForContext<Program>().Fatal(e, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.ConfigureEndpointDefaults(listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
