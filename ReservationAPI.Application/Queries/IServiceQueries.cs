namespace  ReservationAPI.Application.Queries;

public interface IServiceQueries
{
   public Task<List<Service>> GetServiceAsync();
}