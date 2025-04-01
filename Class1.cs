using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationAPI.Application.IntegrationEvents.Events; // Agregar esta línea

namespace ReservationAPI.Application.IntegrationEvents
{
    public interface IReservationIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
