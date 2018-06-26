﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIdMigration.Common;
using SimpleBus.InMemory;
using SimpleIdentityServer.AccessToken.Store.InMemory;
using SimpleIdentityServer.Authenticate.Basic;
using SimpleIdentityServer.EF;
using SimpleIdentityServer.EF.InMemory;
using SimpleIdentityServer.EventStore.Handler;
using SimpleIdentityServer.EventStore.InMemory;
using SimpleIdentityServer.Host;
using SimpleIdentityServer.Shell;
using SimpleIdentityServer.Store.InMemory;
using SimpleIdentityServer.UserManagement;

namespace WebSiteAuthentication.OpenIdProviderOffline
{
    public class Startup
    {
        private IdentityServerOptions _options;
        private IHostingEnvironment _env;
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            _options = new IdentityServerOptions
            {
                Authenticate = new AuthenticateOptions
                {
                    CookieName = Constants.CookieName
                },
                Scim = new ScimOptions
                {
                    IsEnabled = false
                }
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // 2. Add the dependencies needed to enable CORS
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            // 3. Configure Simple identity server
            ConfigureEventStoreInMemoryBus(services);
            ConfigureOauthRepositoryInMemory(services);
            ConfigureStorageInMemory(services);
            ConfigureLogging(services);
            services.AddInMemoryAccessTokenStore(); // Add the access token into the memory.
            services.AddAuthentication(Constants.CookieName)
                .AddCookie(Constants.CookieName, opts =>
                {
                    opts.LoginPath = "/Authenticate";
                });
            services.AddAuthorization(opts =>
            {
                opts.AddOpenIdSecurityPolicy(Constants.CookieName);
            });
            // 5. Configure MVC
            var mvcBuilder = services.AddMvc();
            services.AddOpenIdApi(_options); // API
            services.AddBasicShell(mvcBuilder, _env, new BasicShellOptions
            {
                Descriptors = new[] { BasicAuthenticateModule.ModuleUi, UserManagementModule.ModuleUi }
            });  // SHELL
            services.AddBasicAuthentication(mvcBuilder, _env, new BasicAuthenticateOptions
            {
                IsScimResourceAutomaticallyCreated = false
            });  // BASIC AUTHENTICATION
            services.AddUserManagement(mvcBuilder, _env, new UserManagementOptions());  // USER MANAGEMENT
        }

        private void ConfigureEventStoreInMemoryBus(IServiceCollection services)
        {
            services.AddEventStoreInMemoryEF();
            services.AddSimpleBusInMemory();
            services.AddEventStoreBusHandler(new EventStoreHandlerOptions(ServerTypes.OPENID));
        }
        
        private void ConfigureOauthRepositoryInMemory(IServiceCollection services)
        {
            services.AddOAuthInMemoryEF();
        }

        private void ConfigureStorageInMemory(IServiceCollection services)
        {
            services.AddInMemoryStorage();
        }

        private void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseAuthentication();
            //1 . Enable CORS.
            app.UseCors("AllowAll");
            // 2. Use static files.
            app.UseShellStaticFiles();
            // 3. Redirect error to custom pages.
            app.UseStatusCodePagesWithRedirects("~/Error/{0}");
            // 4. Enable SimpleIdentityServer
            app.UseOpenIdApi(_options, loggerFactory);
            // 5. Configure ASP.NET MVC
            app.UseMvc(routes =>
            {
                routes.UseUserPasswordAuthentication();
                routes.UseUserManagement();
                routes.UseShell();
            });
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var simpleIdentityServerContext = serviceScope.ServiceProvider.GetService<SimpleIdentityServerContext>();
                simpleIdentityServerContext.Database.EnsureCreated();
                simpleIdentityServerContext.EnsureSeedData();
            }
        }
    }
}
