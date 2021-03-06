﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIdMigration.Common;
using SimpleBus.Core;
using SimpleIdentityServer.AccessToken.Store.InMemory;
using SimpleIdentityServer.Authenticate.Basic;
using SimpleIdentityServer.Authenticate.LoginPassword;
using SimpleIdentityServer.Authenticate.SMS;
using SimpleIdentityServer.EF;
using SimpleIdentityServer.EF.InMemory;
using SimpleIdentityServer.Host;
using SimpleIdentityServer.Shell;
using SimpleIdentityServer.Store.InMemory;
using SimpleIdentityServer.TwoFactorAuthentication.Twilio;
using SimpleIdentityServer.UserManagement;
using WebSiteAuthentication.OpenIdProvider.Services;

namespace WebSiteAuthentication.OpenIdProvider
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
            ConfigureOauthRepositoryInMemory(services);
            ConfigureStorageInMemory(services);
            ConfigureLogging(services);
            ConfigureBus(services);
            services.AddInMemoryAccessTokenStore(); // Add the access token into the memory.
            services.AddAuthentication(Constants.ExternalCookieName)
                .AddCookie(Constants.ExternalCookieName)
                .AddFacebook(opts =>
                {
                    opts.ClientId = "569242033233529";
                    opts.ClientSecret = "12e0f33817634c0a650c0121d05e53eb";
                    opts.SignInScheme = Constants.ExternalCookieName;
                    opts.Scope.Add("public_profile");
                    opts.Scope.Add("email");
                });
            services.AddAuthentication("SimpleIdentityServer-TwoFactorAuth")
                .AddCookie("SimpleIdentityServer-TwoFactorAuth");
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
            services.AddBasicShell(mvcBuilder, _env);  // SHELL
            services.AddTwoFactorSmsAuthentication(new TwoFactorTwilioOptions
            {
                TwilioAccountSid = "",
                TwilioAuthToken = "",
                TwilioFromNumber = "",
                TwilioMessage = "The activation code is {0}"
            }); // SMS TWO FACTOR AUTHENTICATION.
            services.AddLoginPasswordAuthentication(mvcBuilder, _env, new BasicAuthenticateOptions
            {
                IsScimResourceAutomaticallyCreated = false
            });  // LOGIN & PASSWORD
            services.AddSmsAuthentication(mvcBuilder, _env, new SmsAuthenticationOptions
            {
                Message = "The activation code is {0}",
                TwilioSmsCredentials= new SimpleIdentityServer.Twilio.Client.TwilioSmsCredentials
                {
                    AccountSid = "AC00101f6a209ad3a50d818707d4d95954",
                    AuthToken = "4bc240a797d11cb7d325a97c787f434c",
                    FromNumber = "+41798073234"
                },
                IsScimResourceAutomaticallyCreated = false
            }); // SMS AUTHENTICATION.
            services.AddUserManagement(mvcBuilder, _env, new UserManagementOptions());  // USER MANAGEMENT
        }

        private void ConfigureBus(IServiceCollection services)
        {
            services.AddTransient<IEventPublisher, EventPublisher>();
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
                routes.UseLoginPasswordAuthentication();
                routes.UseUserManagement();
                routes.UseShell();
                routes.MapRoute("AuthArea",
                    "{area:exists}/Authenticate/{action}/{id?}",
                    new { controller = "Authenticate", action = "Index" });
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