using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
namespace ReservationAPI.Domain.Events;

    public record CreateReservation (Reservation reservation  ) : IDomainEvent;

public interface IDomainEvent
{
}