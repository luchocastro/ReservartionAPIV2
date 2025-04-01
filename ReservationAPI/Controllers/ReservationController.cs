using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationAPI.Application.Commands;
using ReservationAPI.Application.DTOs;
using ReservationAPI.Application.Queries;
using System.Diagnostics;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static ReservationAPI.Application.Commands.CreateReservationCommand;
using Service = ReservationAPI.Application.DTOs.Service;

namespace ReservationAPI.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReservationController> _logger;
        private readonly IReservationQueries _reservationQueries;
        private readonly IServiceQueries _serviceQueries; 
        private readonly IRequestHandler<CreateReservationCommand,
          ReservationDTO> _createReservationCommandHandler;
        public ReservationController(
            IMediator mediator,
            ILogger<ReservationController> logger,
            IReservationQueries reservationQueries,
            IServiceQueries serviceQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reservationQueries = reservationQueries  ?? throw new ArgumentNullException(nameof(reservationQueries));
            _serviceQueries = serviceQueries ?? throw new ArgumentNullException(nameof(serviceQueries));
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
                //Todo: It's good idea to take advantage of GetOrderByIdQuery and handle by GetCustomerByIdQueryHandler
                //var order customer = await _mediator.Send(new GetOrderByIdQuery(orderId));
                var reservation = await _reservationQueries.GetFreeHours(date);

                return Ok(reservation);
            }
            catch (Exception ex)
            {


                return NotFound();
            }
        }
        [Route("servicies")]
        [HttpGet]
        [ProducesResponseType(typeof(Service), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetServiciesAsync()
        {
            try
            {
                var reservation = await _serviceQueries.GetServiceAsync();
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [Route("reservation")]
        [HttpPost]
        public async Task<ActionResult<ReservationDTO>> CreateReservationDataAsync([FromBody] CreateReservationCommand createReservationCommand)
        {
            try
            {
                await _createReservationCommandHandler.Handle(createReservationCommand, CancellationToken.None);
                return Ok(createReservationCommand);

            }
            catch {
                return UnprocessableEntity();
                    }



        }
    }
}