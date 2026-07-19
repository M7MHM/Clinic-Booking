using Clinic.Domain.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.interfaces.repos
{
    public interface IAppointmentRepo
    {
        Task<Appointment?> GetAppointmentByIdAsync(Guid id);
        Task AddAppointmentAsync(Appointment appointment);
        Task RemoveAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAppointmentByDoctorIdAsync(Guid doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentByPatientIdAsync(Guid patientId);
        Task UpdateAppointmentAsync(Appointment appointment);
    }
}
