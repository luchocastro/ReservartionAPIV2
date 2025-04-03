

using Dapper;
using Google.Protobuf.Collections;
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
    }

    public async Task<int> GetQtyResrevationByName(string sdate, string name)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Reservation WHERE date=@date";
        var Reservations = await connection.QueryAsync<Reservation>(sql, new { date = sdate});
        return await Task.FromResult(Reservations.Where(x => x.ClientName == name).Count());
    }

    public async Task<IEnumerable<Reservation>> GetReservations()
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            var sql = "SELECT * FROM Reservation";
            // Use the Query method to execute the query and return a list of objects    
            return await connection.QueryAsync<Reservation>(sql);

        }
        catch (Exception ex)
        {
            //if (ex.Message.ToLower().Contains("no such table"))
            //{
            //    connection.ExecuteAsync("Create Table Service (ServiceId,Name)");

            //    connection.ExecuteAsync("Insert into Service (ServiceId, Name) Values ( 'Corte', 'Corte'), ( 'Corte y Afeitado', 'Corte y Afeitado')" +
            //    ", ('Rasurado', 'Rasurado'), ( 'Afeitado', 'Afeitado'), ( 'Manicura', 'Manicura')");
            //    connection.ExecuteAsync("Create Table Reservation (Id,ClientName, Date, Hour, Service)");
            //}
            throw (ex);
        }
       // return await Task.FromResult(List);
         
    }
}


    

