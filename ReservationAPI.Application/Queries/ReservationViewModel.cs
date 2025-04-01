
namespace ReservationAPI.Application.Queries;

public record Reservation
{
    public string Id { get; init; }
    public string ClientName { get; init; }
    public DateOnly Date { get; init; }
    public string Hour { get; init; }
    public string Service { get; init; }
}


