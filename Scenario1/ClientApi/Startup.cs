using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Uma.Authorization;
using SimpleIdentityServer.UmaIntrospection.Authentication;

namespace ClientApi
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
                options.AddPolicy("uma", policy =>
                {
                    // policy.Requirements.Add(new ConventionalUmaAuthorizationRequirementTst(null));
                    policy.AddConventionalUma();
                    // options.AddPolicy("resourceSet", policy => policy.AddResourceUma("<url>", "<read>","<update>"));
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
                OpenIdWellKnownConfigurationUrl = "https://localhost:5443/.well-known/openid-configuration",
                ResourcesUrl = "https://localhost:5444/api/vs/resources",
                UmaConfigurationUrl = "https://localhost:5445/.well-known/uma-configuration"
            };
            app.UseAuthenticationWithUmaIntrospection(options);
            app.UseMvc();
        }
    }
}
