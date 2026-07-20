using Clinic.Domain.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.interfaces.repos
{
    public interface IDoctorRepo
    {
        Task<List<Doctor>> AllDoctorsAsync();
        Task<Doctor?> GetDoctorByIdAsync(Guid id);
        Task AddDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
    }
}
