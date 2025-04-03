using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationAPI.Domain.Events;
public class ReservationStartDomainEvent : INotification
{
    private string _name;
    private DateOnly _date;
    private bool _haveReservation;
    public string Name { get { return _name; } }
    public DateOnly Date { get { return _date; } }
    public bool HaveReservation { get { return _haveReservation;  }set { _haveReservation = value; } }
    public ReservationStartDomainEvent(string name, DateOnly date)
    {
        _name = name;
        _date = date;
    }
}