
using ReservationAPI.Domain.Common;
using System;

namespace ReservationAPI.Domain.AggregatesModel.AggregateReservation
{
    public class Reservation :Entity, IAggregateRoot
    {
        public string ClientName { get; private set; }
        public DateOnly Date { get; private set; }
        public string Hour { get; private set; }
        public string Service { get; private set; }
        public Reservation(string id, string clientName, DateOnly date, string hour, string service)
        {
            ClientName = clientName;
            Date = date;
            Hour = hour;
            Id = id;
            Service = service;
        }
    }
}