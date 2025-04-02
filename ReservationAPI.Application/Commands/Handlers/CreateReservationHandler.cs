
using ReservationAPI.Application.DTOs;
using ReservationAPI.Domain.AggregatesModel.AggregateReservation;
using ReservationAPI.Domain.Events;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ReservationAPI.Application.Commands.Handlers;


    

    public class CreateReservationCommandHandler : ReservationAPI.Application.Commands.Interface.ICommandHandler<CreateReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;

    public CreateReservationCommandHandler(IReservationRepository reservationRepository)
        {
        _reservationRepository = reservationRepository;
        }

        public async Task  HandleAsync(CreateReservationCommand reservationCommand)
        {
            var reservationDTO = new ReservationDTO 
            {
                //Hacer acá la conversión de string a date
                ClientName = reservationCommand.ClientName,
                Date = reservationCommand.Date,
                Hour = reservationCommand.Hour,
                Service = reservationCommand.Service
            };
        DateOnly date = new DateOnly(1, 1, 1);
        var ok = DateOnly.TryParseExact(reservationCommand.Date, Const.DateFormat, out date);
        if (!ok)
        {
            throw new InvalidCastException(Const.DateWithouFormat);
        }
        await _reservationRepository.CreateAsync(new Reservation(Guid.NewGuid().ToString(), reservationDTO.ClientName,
        date, reservationDTO.Hour, reservationDTO.Service));
        await Task.FromResult(reservationDTO);
        }

 

   
}
