
using ReservationAPI.Domain.Common;
using ReservationAPI.Domain.Events;
using System;
using System.Security.Cryptography.X509Certificates;

namespace ReservationAPI.Domain.AggregatesModel.AggregateReservation
{
    public class Reservation :Entity, IAggregateRoot
    {
        public string ClientName { get; private set; }
        public DateOnly Date { get; private set; }
        public string Hour { get; private set; }
        public string Service { get; private set; }
        public bool HasReservation { get; private set; } = true;
        public Reservation(string id, string clientName, DateOnly date, string hour, string service, int QtyPreviousReservation)
        {
            if (QtyPreviousReservation > 1)
                throw new Exception("No puede servar más de una vez para el mismo día");
            ClientName = clientName;
            Date = date;
            Hour = hour;
            Id = id;
            Service = service;
        }
        public async void HasReservationForDate(IReservationRepository Repo, string clientName, DateOnly date)
        {
            var PreviousReservation = await Repo.GetReservationsByDayOrNameAsync(date, clientName);
            HasReservation= PreviousReservation.Count() > 1;
        }
    }
}