

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Reflection;

namespace ReservationAPI.Application.Queries;

public class ReservationQueries : IReservationQueries
{
    private string _connectionString = "";

    public ReservationQueries(string constr="")
    {
        _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
    }

    public async Task<List<string>> GetFreeHours(string sdate)
    {
        DateOnly date = new DateOnly(1, 1, 1);
        var ok = DateOnly.TryParseExact(sdate, Const.DateFormat, out date);
        if (!ok)
        {
            throw new InvalidCastException(Const.DateWithouFormat);
        }
        var ap = 8;
        var cs = 12;
        var aps = 16;
        var c = 20;
        var TotalHours = Enumerable.Range(ap, cs);
        TotalHours.ToList().AddRange(Enumerable.Range(aps, c));
        var List = new List<string>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = new SqliteCommand("Select Hour from Reservation where [date] = '$fecha'", connection);
        command.Parameters.AddWithValue("$date", sdate);
        command.CommandType = System.Data.CommandType.Text;
        try
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        List.Add( reader.GetString(0));

                    }
                }
            }
            return await Task.FromResult(TotalHours.Where(h => List.ToList().IndexOf(h.ToString()) < 0).Select(x => x.ToString().PadLeft(2, '0') + ":00").ToList());        
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
        //deberìa vernir de la DB
        

        
    }


    public async Task<List<Reservation>> GetReservations()
    {
        var List = new List<Reservation>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = new SqliteCommand("Select * from Reservation", connection);
        command.CommandType = System.Data.CommandType.Text;
        try
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        List.Add(new Reservation { Id = reader.GetString(0), ClientName = reader.GetString(1),
                            Date = DateOnly.FromDateTime(reader.GetDateTime(2)), Hour = reader.GetString(3), Service = reader.GetString(4)
                        });

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


    

