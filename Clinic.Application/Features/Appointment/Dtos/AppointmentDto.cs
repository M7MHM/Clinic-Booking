using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Dtos
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime AppointmentDate { get; set; }

        public string Notes { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string PatientName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
