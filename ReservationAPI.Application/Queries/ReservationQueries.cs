

using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT Hour FROM Reservation WHERE date=@date";
        // Use the Query method to execute the query and return a list of objects    
        var Reservations = await connection.QueryAsync<Reservation>(sql, new { date = sdate }) ;
        var List = Reservations.Select(x => x.Hour);
        return await Task.FromResult(TotalHours.Select(x=>x.ToString().PadLeft(2, '0') + ":00").Where(h => List.ToList().IndexOf(h.ToString()) < 0).Select(x => x).ToList());        
        
        //deberìa vernir de la DB
        

        
    }


    public async Task<IEnumerable<Reservation>> GetReservations()
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "SELECT * FROM Reservation";
            // Use the Query method to execute the query and return a list of objects    
            return await connection.QueryAsync<Reservation>(sql);

        }
        catch (Exception ex)
        {
            //if (ex.Message.ToLower().Contains("no such table"))
            //{

            //    command = new SqliteCommand("Create Table Service (Id,Name)", connection);
            //    command.CommandType = System.Data.CommandType.Text;
            //    command.ExecuteNonQuery();

            //    command = new SqliteCommand("Insert into Service (Id, Name) Values ( '1', 'Corte'), ( '2', 'Corte y Afeitado')" +
            //    ", ('3', 'Rasurado'), ( '4', 'Afeitado'), ( '5', 'Manicura')", connection);
            //    command.CommandType = System.Data.CommandType.Text;
            //    command.ExecuteNonQuery();

            //    command = new SqliteCommand("Create Table Reservation (Id,ClientName, Date, Hour, Service)", connection);
            //    command.CommandType = System.Data.CommandType.Text;
            //    command.ExecuteNonQuery();
            //}
            throw (ex);
        }
       // return await Task.FromResult(List);
         
    }
}


    

