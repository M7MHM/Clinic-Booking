using Clinic.Domain.interfaces.repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorRepo _doctorRepo;
        public DoctorsController(IDoctorRepo doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctor = await _doctorRepo.AllDoctorsAsync();
            return Ok(doctor);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorId(Guid id)
        {
            var doctor = await _doctorRepo.GetDoctorByIdAsync(id);
            return Ok(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> AddDoctor(Doctor doctor)
        {
             await _doctorRepo.AddDoctorAsync(doctor); 
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(Doctor doctor , Guid id)
        {
            if (id != doctor.Id)
                return BadRequest();
            await _doctorRepo.UpdateDoctorAsync(doctor);
            return NoContent();
        }
    }
}
