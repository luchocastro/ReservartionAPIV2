

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Reflection;

namespace ReservationAPI.Application.Queries;

public class ServiceQueries : IServiceQueries
{
    private string _connectionString = "";

    public ServiceQueries(string constr="")
    {
        _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
    }

    public async Task<List<Service>> GetServiceAsync()
    {
        var UnaMas = true;
        var List = new List<Service>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = new SqliteCommand("Select id, name from Service", connection);
        command.CommandType = System.Data.CommandType.Text;
        //var reader = await command.ExecuteReaderAsync();
        DeNuevo:
        try
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        List.Add(new Service { id = reader.GetInt32(0), name = reader.GetString(1) });

                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("no such table"))
            {

                command = new SqliteCommand("Create Table Service (Id,Name)", connection);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();

                command = new SqliteCommand("Insert into Service (Id, Name) Values ( '1', 'Corte'), ( '2', 'Corte y Afeitado')" +
                ", ('3', 'Rasurado'), ( '4', 'Afeitado'), ( '5', 'Manicura')", connection);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();

                command = new SqliteCommand("Create Table Reservation (Id,ClientName, Date, Hour, Service)", connection);
                command.CommandType = System.Data.CommandType.Text;
                command.ExecuteNonQuery();
            }
            throw (ex);
        }
        return await Task.FromResult(List);
         
    }
}


    

