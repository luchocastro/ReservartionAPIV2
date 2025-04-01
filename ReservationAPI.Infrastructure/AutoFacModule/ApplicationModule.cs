

using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using ReservationAPI.Infrastructure.Repositories;

namespace ReservationAPI.Infrastructure.AutoFacModule;

public class ApplicationModule
    : Autofac.Module
{

    public string QueriesConnectionString { get; }

    public ApplicationModule(string qconstr)
    {
        QueriesConnectionString = qconstr;

    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Infrastructure.Repositories.ReservationRepository>()
            .As<IReservationRepository>()
            .InstancePerLifetimeScope();
    }
}