

using Microsoft.Data.Sqlite;

using Dapper;
namespace ReservationAPI.Application.Queries;

public class ServiceQueries : IServiceQueries
{
    private string _connectionString = "";

    public ServiceQueries(string constr="")
    {
        _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
    }

    public async Task<IEnumerable<Service>> GetServiceAsync()
    {

        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryAsync<Service>("Select ServiceId, name from Service");
    }
}


    

