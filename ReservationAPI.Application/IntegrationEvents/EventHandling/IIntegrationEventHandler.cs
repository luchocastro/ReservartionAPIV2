using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationAPI.Application.IntegrationEvents.EventHandling
{
    public interface IIntegrationEventHandler<T>
    {
        Task Handle(T @event);
    }

}
