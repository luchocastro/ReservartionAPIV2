using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationAPI.Infrastructure.Extentions
{
    using MediatR;
    using ReservationAPI.Domain.Common;
    using ReservationAPI.Infrastructure.Context;
    using System.Linq;
    using System.Threading.Tasks;

    namespace Extentions.Infrastructure
    {
        public static class MediatorExtension
        {
            public static async Task DispatchDomainEventsAsync(this IMediator mediator, ReservationContext ctx)
            {
                var domainEntities = ctx.ChangeTracker.Entries<Entity>().Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
                var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
                domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

                var tasks = domainEvents
                    .Select(async (domainEvent) => {
                        await mediator.Publish(domainEvent);
                    });

                await Task.WhenAll(tasks);
            }
        }
    }
}
