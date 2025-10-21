using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace PortalGalaxy.DataAccess
{
    // Aqui usamos Primary Constructor (C# 12)
    public class PortalGalaxyDbContext(DbContextOptions<PortalGalaxyDbContext> options) 
        : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            configurationBuilder.Properties<string>()
                .HaveMaxLength(100);

            //configurationBuilder.Properties<DateTime>()
            //    .HaveColumnType("DATETIME");

            configurationBuilder.Conventions.Remove<SqlServerOnDeleteConvention>();
        }
    }
}
