//using Microsoft.Extensions.Logging;
//using ReservationAPI.Infrastructure.Context;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;
//using ReservationAPI.Application.IntegrationEvents.EventHandling;
//using ReservationAPI.Application.DTOs;
//using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
//using EventLog = ReservationAPI.Infrastructure.Context.EventLog;

//public class ReservationEventHandler : IIntegrationEventHandler<ReservationCreatedIntegrationEvent>
//{
//    private readonly WriteReservationContext _WriteReservationContext;
//    private readonly ILogger<ReservationEventHandler> _logger;
//    private List<Reservation> Reservations = new List<Reservation>();
//    public DbSet<EventLog> EventLogs { get; init; }
    
//    public ReservationEventHandler(WriteReservationContext WriteReservationContext, ILogger<ReservationEventHandler> logger)
//    {
//        _WriteReservationContext = WriteReservationContext ?? throw new ArgumentNullException(nameof(WriteReservationContext));
//        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//    }

//    public async Task Handle(ReservationCreatedIntegrationEvent @event)
//    {
//        _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

//        // Check if the event has already been processed
//        if (await _WriteReservationContext.EventLogs.AnyAsync(e => e.EventId.ToString() == @event.Id))
//        {
//            _logger.LogInformation("Event {IntegrationEventId} already processed", @event.Id);
//            return;
//        }

//        // Process the event
//        var reservation = new Reservation(@event.Id,@event.ClientName, @event.Date, @event.Hour, @event.Service);
//        _WriteReservationContext.Reservations.Add(reservation);

//        // Save the event log
//        _WriteReservationContext.EventLogs.Add(new EventLog { EventId = @event.Id , EventType = @event.GetType().Name });

//        await _WriteReservationContext.SaveChangesAsync();
//    }
//}
