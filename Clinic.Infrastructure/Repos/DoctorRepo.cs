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
    public class DoctorRepo : IDoctorRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public DoctorRepo(AppDbContext context , IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
           await _context.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Doctor>> AllDoctorsAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(Guid id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
