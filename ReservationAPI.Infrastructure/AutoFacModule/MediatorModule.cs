using FluentValidation;
using Mediator;
using ReservationAPI.Domain.Events;

namespace ReservationAPI.Infrastructure.AutoFacModule;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsSelf()
            .AsImplementedInterfaces();

        // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
        builder.RegisterAssemblyTypes(typeof(IDomainEvent).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(IDomainEvent).GetTypeInfo().Assembly)
        .AsClosedTypesOf(typeof(IRequestHandler<,>));
    }
}