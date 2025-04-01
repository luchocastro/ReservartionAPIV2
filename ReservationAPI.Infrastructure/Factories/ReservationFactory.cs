using Microsoft.Extensions.Configuration;
using ReservationAPI.Infrastructure.Context;
public class ReservationFactory : IDesignTimeDbContextFactory<ReservationContext>
    {
        public ReservationContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ReservationContext>();

            optionsBuilder.UseSqlServer(config["ConnectionString"], sqlServerOptionsAction: o => o.MigrationsAssembly("Ordering.API"));

            return new ReservationContext(optionsBuilder.Options);
        }
    }

