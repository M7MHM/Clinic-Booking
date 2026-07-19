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
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public AppointmentRepo(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentByDoctorIdAsync(Guid doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentByPatientIdAsync(Guid patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task RemoveAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
