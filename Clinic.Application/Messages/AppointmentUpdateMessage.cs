using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Application.Messages
{
    public class AppointmentUpdateMessage
    {
        public Guid AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime NewDate { get; set; }
    }
}