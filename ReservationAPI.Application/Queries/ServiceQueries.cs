

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Reflection;
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
        var UnaMas = true;
        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryAsync<Service>("Select id, name from Service");
    }
}


    

