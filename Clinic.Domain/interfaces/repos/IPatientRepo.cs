using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.interfaces.repos
{
    public interface IPatientRepo
    {
         Task<List<Patient>> AllPatientAsync();
         Task<Patient?> GetPatientByIdAsync(Guid id);
         Task AddPatientAsync(Patient patient);
         Task UpdatePatientAsync(Patient patient);
    }
}
