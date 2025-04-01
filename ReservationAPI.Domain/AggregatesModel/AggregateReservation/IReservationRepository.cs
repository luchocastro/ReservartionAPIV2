using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationAPI.Domain.AggregatesModel.AggregateReservation
{
    public interface IReservationRepository
    {
        public Task<IEnumerable<Reservation>> GetReservationsByDayOrNameAsync(DateOnly date, string name);
        public Task<Reservation> CreateAsync(Reservation reservation);
        public Task<Reservation> GetByIdAsync(string id);


    }
}
