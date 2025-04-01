using MediatR;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using ReservationAPI.Application.Commands;
using ReservationAPI.Application.Queries;
using ReservationAPI.Infrastructure.Context;

namespace ReservationAPI.SetUp
{
    public static class DependencyConteiner
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
            services.AddScoped<IMediator, NoMediator>();
            services.AddSingleton<IServiceQueries, ServiceQueries>(sp =>
            {
                var connection = configuration["ConnectionString"] ?? "Data Source=reservation.db";
                return new ServiceQueries(connection);
            });
            services.AddSingleton<IReservationQueries, ReservationQueries>(sp =>
            {
                var connection = configuration["ConnectionString"] ?? "Data Source=reservation.db";
                return new ReservationQueries(connection);
            });
        }
    }
}
