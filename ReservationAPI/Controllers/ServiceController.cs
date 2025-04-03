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
    public class ServiceController : ControllerBase
    {
        private readonly IServiceQueries _serviceQueries; 
    public ServiceController(
            
            IServiceQueries serviceQueries
            )
        {
            _serviceQueries = serviceQueries ?? throw new ArgumentNullException(nameof(serviceQueries));
        }
        
        [Route("services")]
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
    }
}