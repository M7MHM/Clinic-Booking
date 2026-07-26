using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepo _appointmentRepo;
        public AppointmentsController(IAppointmentRepo appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAllDoctorAppointments(Guid doctorId)
        {
            var appointments = await _appointmentRepo.GetAppointmentByDoctorIdAsync(doctorId);
            return Ok(appointments);
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAllPatientAppointments(Guid patientId)
        {
            var appointments = await _appointmentRepo.GetAppointmentByPatientIdAsync(patientId);
            return Ok(appointments);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var appointment = await _appointmentRepo.GetAppointmentByIdAsync(id);
            return Ok(appointment);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddApointment(Appointment appointment)
        {
            await _appointmentRepo.AddAppointmentAsync(appointment);
            return Ok();
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Appointment appointment , Guid id)
        {
            if(id != appointment.Id)
                return BadRequest();
            await _appointmentRepo.UpdateAppointmentAsync(appointment);
            return NoContent();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id , Appointment appointment)
        {
            if(id != appointment.Id)
                return BadRequest();
            await _appointmentRepo.RemoveAppointmentAsync(appointment);
            return NoContent();
        } 
    }
}
