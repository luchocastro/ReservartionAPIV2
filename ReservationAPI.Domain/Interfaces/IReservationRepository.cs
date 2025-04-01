//using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace ReservationAPI.Domain.Interfaces
//{
//    public interface IReservationRepository
//    {
//        Task<IEnumerable<Reservation>> GetAllAsync();
//        Task<Reservation> GetByIdAsync(int id);
//        Task<Reservation> CreateAsync(Reservation reservation);
//        Task<bool> HasReservationForDateTimeAsync(DateTime date, string hour);
//        Task<int> GetClientReservationsCountAsync(string clientName, DateTime date);
//    }
//}