using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SimpleIdentityServer.EF;

namespace OpenIdMigration.Sqlite
{
    public class SimpleIdentityServerDbContextFactory : IDesignTimeDbContextFactory<SimpleIdentityServerContext>
    {
        public SimpleIdentityServerDbContextFactory()
        {
        }

        public SimpleIdentityServerContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleIdentityServerContext>();
            builder.UseSqlite("Data Source=:memory:;Version=3;New=True;",
                optionsBuilder => optionsBuilder.MigrationsAssembly("OpenIdMigration.Sqlite"));
            return new SimpleIdentityServerContext(builder.Options);
        }
    }
}