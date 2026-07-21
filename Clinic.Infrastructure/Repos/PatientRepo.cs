using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Repos
{
    public class PatientRepo : IPatientRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public PatientRepo(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task AddPatientAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Patient>> AllPatientAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(Guid id)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            _context.Patients.Update(patient);
           await  _unitOfWork.SaveChangesAsync();
        }
    }
}
