using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Uma.Authorization;
using SimpleIdentityServer.UmaIntrospection.Authentication;
using System.Collections.Generic;

namespace SamplesApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Authorization policy
            services.AddAuthorization(options =>
            {
                // Add conventional uma authorization
                options.AddPolicy("readsamples", policy =>
                {
                    policy.AddResourceUma("resources/MedicalWebsite/patient/Samples", new List<string> { "read" }); 
                });
            });


            // Add framework services.
            services.AddAuthentication();
            services.AddLogging();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            app.UseStatusCodePages();
            var options = new UmaIntrospectionOptions
            {
                OpenIdWellKnownConfigurationUrl = Constants.OpenidConfigurationUrl,
                ResourcesUrl = Constants.ResourcesUrl,
                UmaConfigurationUrl = Constants.UmaConfigurationUrl
            };
            app.UseAuthenticationWithUmaIntrospection(options);
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
