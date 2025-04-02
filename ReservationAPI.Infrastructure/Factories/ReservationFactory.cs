using Microsoft.Extensions.Configuration;
using ReservationAPI.Infrastructure.Context;
public class ReservationFactory : IDesignTimeDbContextFactory<WriteReservationContext>
    {
        public WriteReservationContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build()
               ;
                
            var optionsBuilder = new DbContextOptionsBuilder<WriteReservationContext>();

            optionsBuilder.UseSqlite(config["ConnectionString"] ?? "Data Source=reservation.db", sqliteOptionsAction: o => o.MigrationsAssembly("ReservationAPI"));

            return new WriteReservationContext(optionsBuilder.Options);
        }
    }

