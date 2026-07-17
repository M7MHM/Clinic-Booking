using Clinic.Domain.interfaces.repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepo _patientRepo;
        public PatientsController(IPatientRepo PatientRepo)
        {
            _patientRepo = PatientRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPatient()
        {
            var patient = await _patientRepo.AllPatientAsync();
            return Ok(patient);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientId(Guid id)
        {
            var patient = await _patientRepo.GetPatientByIdAsync(id);
            return Ok(patient);
        }
        [HttpPost]
        public async Task<IActionResult> AddPatient(Patient patient)
        {
            await _patientRepo.AddPatientAsync(patient);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Patient patient, Guid id)
        {
            if (id != patient.Id)
                return BadRequest();
            await _patientRepo.UpdatePatientAsync(patient);
            return NoContent();
        }
    }
}

