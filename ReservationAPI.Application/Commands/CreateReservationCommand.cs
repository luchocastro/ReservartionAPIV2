using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Runtime.Serialization;
using ReservationAPI.Application.DTOs;
using ICommand=ReservationAPI.Application.Commands.Interface.ICommand;

namespace ReservationAPI.Application.Commands;

public class CreateReservationCommand
    : ICommand
{
    
    public string ClientName { get;  set; }

    public string Date { get;  set; }

    public string Hour { get;  set; }

    public string ServiceId { get;  set; }  

    public CreateReservationCommand(string clientName, string date, string hour, string serviceId)
    {
        ClientName = clientName;
        Date = date;
        Hour = hour;
        ServiceId = serviceId;
    }



}