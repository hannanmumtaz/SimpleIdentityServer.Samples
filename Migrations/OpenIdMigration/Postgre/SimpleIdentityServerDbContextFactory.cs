using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SimpleIdentityServer.EF;

namespace OpenIdMigration.Postgre
{
    public class SimpleIdentityServerDbContextFactory : IDesignTimeDbContextFactory<SimpleIdentityServerContext>
    {
        public SimpleIdentityServerDbContextFactory()
        {
        }

        public SimpleIdentityServerContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleIdentityServerContext>();
            builder.UseNpgsql("User ID=postgres;Password=password;Host=localhost;Port=5432;Database=idserver;Pooling=true;",
                optionsBuilder => optionsBuilder.MigrationsAssembly("OpenIdMigration.Postgre"));
            return new SimpleIdentityServerContext(builder.Options);
        }
    }
}