using Clinic.Application.Features.Appointment.Commands;
using Clinic.Application.Features.Appointment.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAllDoctorAppointments(Guid doctorId)
        {
            var appointments =
                await _mediator.Send(
                    new GetAppointmentsByDoctorIdQuery(doctorId));

            return Ok(appointments);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAllPatientAppointments(Guid patientId)
        {
            var appointments =
                await _mediator.Send(
                    new GetAppointmentsByPatientQuery(patientId));

            return Ok(appointments);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var appointment =
                await _mediator.Send(
                    new GetAppointmentByIdQuery(id));

            if (appointment == null)
                return NotFound();

            return Ok(appointment);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAppointment(
            [FromBody] CreateAppointmentCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(
            Guid id,
            [FromBody] UpdateAppointmentCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);

            return NoContent();
        }
    }
}