using ManageScimResources.ScimStartup.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleBus.Core;
using SimpleIdentityServer.OAuth2Introspection;
using SimpleIdentityServer.Scim.Db.EF;
using SimpleIdentityServer.Scim.Db.EF.InMemory;
using SimpleIdentityServer.Scim.Host.Extensions;
using SimpleIdentityServer.Scim.Startup.Services;
using SimpleIdentityServer.UserInfoIntrospection;
using WebApiContrib.Core.Concurrency;
using WebApiContrib.Core.Storage.InMemory;

namespace ManageScimResources.ScimStartup
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        public IConfigurationRoot Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(OAuth2IntrospectionOptions.AuthenticationScheme)
                .AddOAuth2Introspection()
		        .AddUserInfoIntrospection(opts =>
                {
                    opts.WellKnownConfigurationUrl = "http://localhost:60000/.well-known/openid-configuration";
                });
            services.AddAuthorization(opts =>
            {
                opts.AddScimAuthPolicy();
            });
            ConfigureBus(services);
            ConfigureScimRepository(services);
            ConfigureCachingInMemory(services);
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddMvc();
            services.AddScimHost();
        }

        private void ConfigureBus(IServiceCollection services)
        {
            services.AddTransient<IEventPublisher, DefaultEventPublisher>();
            /*
            services.AddSimpleBusInMemory(new SimpleBus.Core.SimpleBusOptions
            {
                ServerName = "scim"
            });
            */
        }

        private void ConfigureScimRepository(IServiceCollection services)
        {
            services.AddScimInMemoryEF();
        }

        private void ConfigureCachingInMemory(IServiceCollection services)
        {
            services.AddConcurrency(opt => opt.UseInMemory());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var scimDbContext = serviceScope.ServiceProvider.GetService<ScimDbContext>();
                scimDbContext.Database.EnsureCreated();
                scimDbContext.EnsureSeedData();
            }
        }
    }
}
