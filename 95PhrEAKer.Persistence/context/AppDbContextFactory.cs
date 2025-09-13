using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace _95PhrEAKer.Persistence.context
{

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Use MySQL for design-time services
            optionsBuilder.UseMySql(
                "Your_MySQL_Connection_String_Here",
                new MySqlServerVersion(new Version(8, 0, 28)), // Adjust version as needed
                mySqlOptions => mySqlOptions.MigrationsAssembly("95PhrEAKer.Persistence")
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }

}
