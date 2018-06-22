using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SimpleIdentityServer.EF;

namespace OpenIdMigration.SqlServer
{
    public class SimpleIdentityServerDbContextFactory : IDesignTimeDbContextFactory<SimpleIdentityServerContext>
    {
        public SimpleIdentityServerDbContextFactory()
        {
        }

        public SimpleIdentityServerContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleIdentityServerContext>();
            builder.UseSqlServer("Data Source=.;Initial Catalog=idserver;Integrated Security=True;",
                optionsBuilder => optionsBuilder.MigrationsAssembly("OpenIdMigration.SqlServer"));
            return new SimpleIdentityServerContext(builder.Options);
        }
    }
}