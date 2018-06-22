using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIdMigration.Common;
using SimpleIdentityServer.EF;
using SimpleIdentityServer.EF.Sqlite;
using System;

namespace OpenIdMigration.Sqlite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = GetConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("the connectionString must be specified");
                return;
            }

            Console.WriteLine("=== START TO UPDATE THE OPENID DATABASE ===");
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOAuthSqliteEF(connectionString, s => s.MigrationsAssembly("OpenIdMigration.SqlServer"));
            var serviceProvider = serviceCollection.BuildServiceProvider();
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var simpleIdentityServerContext = serviceScope.ServiceProvider.GetService<SimpleIdentityServerContext>();
                simpleIdentityServerContext.Database.Migrate();
                simpleIdentityServerContext.EnsureSeedData();
            }

            Console.WriteLine("=== FINISH TO UPDATE THE OPENID DATABASE ===");
        }

        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            return configuration["ConnectionString"];
        }
    }
}