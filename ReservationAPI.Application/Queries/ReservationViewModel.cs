
namespace ReservationAPI.Application.Queries;

public record Reservation
{
    public string Id { get; set; }
    public string ClientName { get; set; }
    public string Date { get; set; }
    public string Hour { get; set; }
    public string Service { get; set; }
}


