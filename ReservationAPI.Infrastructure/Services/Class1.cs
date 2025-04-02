using Microsoft.Data.Sqlite;
using ReservationAPI.Infrastructure.Context;
using ReservationAPI.Infrastructure.Context.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReservationAPI.Infrastructure.Services
{
    internal sealed class ReservationReadService// : ISampleEntityReadService
    {
        private readonly DbSet<ReadReservation> _Reservations;

        public ReservationReadService(ReadReservationContext context)
            => _Reservations = context.Reservations;
    
        public Task<bool> ExistsByNameAsync(string name, string date)
            => _Reservations.AnyAsync(pl => pl.ClientName == name & pl.Date==date);

        public async Task<List<string>> HoursAvailable(string name, string sdate)
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
            var List = _Reservations.Where(x => x.Date == sdate).Select(x => x.Hour);
            return await Task.FromResult(TotalHours.Where(h => List.ToList().IndexOf(h.ToString()) < 0).Select(x => x.ToString().PadLeft(2, '0') + ":00").ToList());
            
        }
    }
}
