namespace  ReservationAPI.Application.Queries;

public interface IReservationQueries
{
   public Task<List<Reservation>> GetReservations();
    public Task<List<String>> GetFreeHours(string date);
}