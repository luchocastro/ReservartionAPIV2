namespace  ReservationAPI.Application.Queries;

public interface IReservationQueries
{
   public Task<IEnumerable<Reservation>> GetReservations();
    public Task<List<String>> GetFreeHours(string date);
}