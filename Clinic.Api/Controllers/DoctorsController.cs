using Clinic.Application.Features.Doctor.Commands;
using Clinic.Application.Features.Doctor.Queries;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctor = await _mediator.Send(new GetAllDoctorQuery());
            return Ok(doctor);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorId(Guid id)
        {
            var doctor = await _mediator.Send(new GetDoctorByIdQuery(id));
            if (doctor == null)
                return NotFound();
            return Ok(doctor);
        }
        [Authorize(Roles = "Doctor,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] CreateDoctorCommand command)
        {
           var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Authorize(Roles = "Doctor,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(Guid id , UpdateDoctorCommand command)
        {
            if (id != command.Id)
                return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
