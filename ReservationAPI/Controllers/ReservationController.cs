using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Application.Commands;
using ReservationAPI.Application.Commands.Interface;
using ReservationAPI.Application.DTOs;
using ReservationAPI.Application.Queries;
using System.Diagnostics;
using System.Net;
using static ReservationAPI.Application.Commands.CreateReservationCommand;
using Service = ReservationAPI.Application.DTOs.Service;

namespace ReservationAPI.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationQueries _reservationQueries;
        private readonly ICommandHandler<CreateReservationCommand> _createReservationCommandHandler;
        public ReservationController(
            ILogger<ReservationController> logger,
            IReservationQueries reservationQueries,
            ICommandHandler<CreateReservationCommand> createReservationCommandHandler
            )
        {
            _reservationQueries = reservationQueries  ?? throw new ArgumentNullException(nameof(reservationQueries));
            _createReservationCommandHandler= createReservationCommandHandler ?? throw new ArgumentNullException(nameof(_createReservationCommandHandler));
        }
        [Route("reservations")]
        [HttpGet]
        [ProducesResponseType(typeof(Service), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReservationAsync()
        {
            try
            {
                //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
                //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
                var reservation = await _reservationQueries.GetReservations ();

                return Ok(reservation);
            }
            catch (Exception ex)
            {


                return NotFound();
            }
        }

        [Route("hours")]
        [HttpGet]
        [ProducesResponseType(typeof(Service), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetHoursAsync(string date )
        {
            try
            {
              var reservation = await _reservationQueries.GetFreeHours(date);

                return Ok(reservation);
            }
            catch (Exception ex)
            {


                return NotFound();
            }
        }
        [Route("CreateReservation")]
        [HttpPost]
        public async Task<ActionResult<ReservationDTO>> CreateReservationDataAsync([FromBody] CreateReservationCommand createReservationCommand)
        {
            try
            {
                await _createReservationCommandHandler.HandleAsync(createReservationCommand);
                return Ok(createReservationCommand);
            }
             catch {
                return UnprocessableEntity();
                    }
        }
    }
}