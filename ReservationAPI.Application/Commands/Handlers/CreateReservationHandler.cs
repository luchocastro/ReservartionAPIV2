
using ReservationAPI.Application.DTOs;
using ReservationAPI.Application.Queries;
using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using Reservation= ReservationAPI.Domain.AggregatesModel.AggregateReservation.Reservation;
using ReservationAPI.Domain.Events;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ReservationAPI.Application.Commands.Handlers;


    

    public class CreateReservationCommandHandler : ReservationAPI.Application.Commands.Interface.ICommandHandler<CreateReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationQueries _reservationQueries;
    public CreateReservationCommandHandler(IReservationRepository reservationRepository, IReservationQueries reservationQueries)
    {
        _reservationRepository = reservationRepository;
        _reservationQueries = reservationQueries;
        }

        public async Task  HandleAsync(CreateReservationCommand reservationCommand)
        {

            var reservationDTO = new ReservationDTO 
            {
                //Hacer acá la conversión de string a date
                ClientName = reservationCommand.ClientName,
                Date = reservationCommand.Date,
                Hour = reservationCommand.Hour,
                Service = reservationCommand.ServiceId
            };
        DateOnly date = new DateOnly(1, 1, 1);
        var ok = DateOnly.TryParseExact(reservationCommand.Date, Const.DateFormat, out date);
        if (!ok)
        {
            throw new InvalidCastException(Const.DateWithouFormat);
        }
        var reservations = await  _reservationQueries.GetQtyResrevationByName(reservationCommand.Date, reservationCommand.ClientName);
        await _reservationRepository.CreateAsync(new Reservation(Guid.NewGuid().ToString(), reservationDTO.ClientName,
        date, reservationDTO.Hour, reservationDTO.Service, reservations));
        await Task.FromResult(reservationDTO);
        }

 

   
}
