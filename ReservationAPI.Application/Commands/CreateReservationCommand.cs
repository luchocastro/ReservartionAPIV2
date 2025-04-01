using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Runtime.Serialization;
using ReservationAPI.Application.DTOs;
namespace ReservationAPI.Application.Commands;
using MediatR;
public class CreateReservationCommand
    : IRequest<ReservationDTO>
{
   
    public string Id { get;  set; }


    public string ClientName { get;  set; }

    public string Date { get;  set; }

    public string Hour { get;  set; }

    public string Service { get;  set; }

    public CreateReservationCommand(string id, string clientName, string date, string hour, string service) 
    {
        Id = id;
        ClientName = clientName;
        Date = date;
        Hour = hour;
        Service = service;
    }



}