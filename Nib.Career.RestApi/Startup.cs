using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nib.Career.GrpcServer.V1;
using Nib.Career.RestApi.Configuration;
using Nib.Career.RestApi.HealthChecks;
using Serilog;

namespace Nib.Career.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddControllers(options =>
            {
                options.Filters.Add<UnhandledExceptionFilter>();
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.RegisterMiddleware = true;
            })
            .AddVersionedApiExplorer(options => { options.SubstituteApiVersionInUrl = true; });

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            services.AddSingleton<HealthCheckStatusData>();
            services.AddHealthChecks()
                .AddCheck<LivenessHealthCheck>("Liveness")
                .AddCheck<ReadinessHealthCheck>("Readiness");

            var url = new Uri(Configuration.GetSection(nameof(CareerGrpcApiOptions)).Get<CareerGrpcApiOptions>().BaseUrl);

            services.AddGrpcClient<JobDetailsService.JobDetailsServiceClient>(options => options.Address = url);
            services.AddGrpcClient<LocationService.LocationServiceClient>(options => options.Address = url);

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

            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            app.UseRouting();
            app.UseHsts();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
