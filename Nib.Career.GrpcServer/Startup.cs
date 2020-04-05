using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nib.Career.Core.Configs;
using Nib.Career.Core.Helpers;
using Nib.Career.Core.Services;
using Nib.Career.GrpcServer.HealthChecks;
using Nib.Career.GrpcServer.V1;
using Serilog;
using System.Reflection;

namespace Nib.Career.GrpcServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddOptions();

            services.AddSingleton<HealthCheckStatusData>();
            services.AddHealthChecks()
                .AddCheck<LivenessHealthCheck>("Liveness")
                .AddCheck<ReadinessHealthCheck>("Readiness");

            services.AddSingleton(typeof(ISimpleMemoryCache), typeof(SimpleMemoryCache));

            ConfigureFileStorageServices(services);
            ConfigureLocationApiServices(services);
        }

        protected virtual void ConfigureFileStorageServices(IServiceCollection services) 
        {
            services.Configure<FileStorageOptions>(Configuration.GetSection(nameof(FileStorageOptions)));
            services.AddSingleton(typeof(IFileStorageService<>), typeof(FileStorageService<>));
        }

        protected virtual void ConfigureLocationApiServices(IServiceCollection services)
        {
            services.Configure<LocationApiOptions>(Configuration.GetSection(nameof(LocationApiOptions)));
            services.AddSingleton(typeof(ILocationApiService), typeof(LocationApiService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/", new HealthCheckOptions { Predicate = check => check.Name == "Liveness" });
            app.UseHealthChecks("/ping", new HealthCheckOptions { Predicate = check => check.Name == "Liveness" });
            app.UseHealthChecks("/live", new HealthCheckOptions { Predicate = check => check.Name == "Liveness" });
            app.UseHealthChecks("/ready", new HealthCheckOptions { Predicate = check => check.Name == "Readiness" });

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<JobDetailsServiceImpl>();
                endpoints.MapGrpcService<LocationServiceImpl>();
            });
        }
    }
}
