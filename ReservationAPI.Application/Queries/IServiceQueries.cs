namespace  ReservationAPI.Application.Queries;

public interface IServiceQueries
{
   public Task<IEnumerable<Service>> GetServiceAsync();
}