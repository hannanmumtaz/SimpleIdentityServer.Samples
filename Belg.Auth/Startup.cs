using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WsFederation;

namespace Belg.Auth
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
            services.AddAuthentication(opts => opts.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddLogging();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            app.UseStatusCodePages();

            // 1. Enable cookie authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/Authenticate")
            });

            // 2. Enable WSFederation authentication
            app.UseWsFedAuthentication(options =>
            {
                options.IdPEndpoint = "https://www.e-contract.be/eid-idp/protocol/ws-federation/auth-ident";
                options.AuthenticationScheme = "EID";
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.AutomaticChallenge = true;
                options.AutomaticAuthenticate = true;
            });
            app.UseMvc();
        }
    }
}
