using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using ReservationAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationAPI.Infrastructure.Repositories;
    public class ReservationRepository : IReservationRepository
    {
        private static readonly List<Reservation> _reservations = new List<Reservation>();
    private readonly WriteReservationContext _writeReservationContext;


    public ReservationRepository(WriteReservationContext writeReservationContext)
    {
        _writeReservationContext = writeReservationContext;
    }

        public async Task<Reservation> GetByIdAsync(string id)
        {
            return await Task.FromResult(_reservations.FirstOrDefault(r => r.Id == id));
        }

        public async Task<Reservation> CreateAsync(Reservation reservation)
        {

            var x = await _writeReservationContext.AddAsync(reservation);
        _writeReservationContext.SaveChanges();
            return await Task.FromResult(reservation);
        }

     
        public async Task<IEnumerable<Domain.AggregatesModel.AggregateReservation.Reservation>> GetReservationsByDayOrNameAsync(DateOnly date, string name)
        {
            return await Task.FromResult(_reservations.Where(z => z.Date == date & (z.ClientName==name | string.IsNullOrEmpty(name))));
        }


        Task<Domain.AggregatesModel.AggregateReservation.Reservation> IReservationRepository.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
