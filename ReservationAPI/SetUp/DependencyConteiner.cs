using Google.Protobuf;
using Mediator;
using MediatR;

using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReservationAPI.Application.Commands;
using ReservationAPI.Application.Commands.Handlers;
using ReservationAPI.Application.Queries;
using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using ReservationAPI.Infrastructure.Context;
using ReservationAPI.Infrastructure.Repositories;
using System.Runtime.InteropServices;
namespace ReservationAPI.SetUp
{
    public static class DependencyConteiner
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
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
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ReservationAPI.Application.Commands.Interface.ICommandHandler<CreateReservationCommand>, CreateReservationCommandHandler>();
            services.AddDbContext<WriteReservationContext>(ctx => ctx.UseSqlite(configuration["ConnectionString"] ?? "Data Source=reservation.db"));
        }
    }
}
