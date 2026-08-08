using Clinic.Application.Features.Patient.Commands;
using Clinic.Application.Features.Patient.Queries;
using Clinic.Application.Features.Patient.Queries.Clinic.Application.Patients.Queries;
using Clinic.Application.Patients.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await _mediator.Send(new GetAllPatientsQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientId(Guid id)
        {
            var result = await _mediator.Send(new GetPatientByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] CreatePatientCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientCommand command)
        {
            if (id != command.Id) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }
    }
}