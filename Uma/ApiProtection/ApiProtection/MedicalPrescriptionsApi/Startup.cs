using ApiProtection.MedicalPrescriptionsApi.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.AccessToken.Store.InMemory;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Uma.Client;
using SimpleIdentityServer.UserInfoIntrospection;

namespace ApiProtection.MedicalPrescriptionsApi
{

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            Configure(services);
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddAuthentication(UserInfoIntrospectionOptions.AuthenticationScheme)
                .AddUserInfoIntrospection(opts =>
                {
                    opts.WellKnownConfigurationUrl = Constants.OpenidWellKnownConfiguration;
                });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("connected", policy => policy.RequireAuthenticatedUser());
            });
            services.AddMvc();
        }

        private static void Configure(IServiceCollection services)
        {
            services.AddIdServerClient();
            services.AddUmaClient();
            services.AddInMemoryAccessTokenStore();
            services.AddTransient<IMedicalPrescriptionStore, MedicalPrescriptionStore>();
            services.AddTransient<IMedicalRecordStore, MedicalRecordStore>();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<MedicalPrescriptionDbContext>(options => options.UseInMemoryDatabase().ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            app.UseCors("AllowAll");
            loggerFactory.AddConsole();
            app.UseStatusCodePages();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MedicalPrescriptionDbContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
