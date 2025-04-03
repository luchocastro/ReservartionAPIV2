namespace  ReservationAPI.Application.Queries;

public interface IReservationQueries
{
   public Task<IEnumerable<Reservation>> GetReservations();
    public Task<List<String>> GetFreeHours(string date);
    public Task<int> GetQtyResrevationByName(string date, string name);
}