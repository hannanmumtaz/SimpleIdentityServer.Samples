using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.HierarchicalResource.EF;
using SimpleIdentityServer.HierarchicalResource.EF.InMemory;
using SimpleIdentityServer.HierarchicalResource.Host.Extensions;
using WebsiteProtection.HierarchicalResource.Extensions;
using System;

namespace WebsiteProtection.HierarchicalResource
{

    public class Startup
    {
        private HierarchicalResourceOptions _options;

        public Startup(IHostingEnvironment env)
        {
            _options = new HierarchicalResourceOptions();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterDatabase(services);
            RegisterHost(services);
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAll");
            loggerFactory.AddConsole();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var resourceManagerContext = serviceScope.ServiceProvider.GetService<HierarchicalResourceDbContext>();
                try
                {
                    resourceManagerContext.Database.EnsureCreated();
                }
                catch (Exception) { }
                resourceManagerContext.EnsureSeedData();
            }
        }

        private void RegisterHost(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHierarchicalResourceHost(_options);
        }

        private void RegisterDatabase(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHierarchicalResourceInMemoryEF();
        }
    }
}
