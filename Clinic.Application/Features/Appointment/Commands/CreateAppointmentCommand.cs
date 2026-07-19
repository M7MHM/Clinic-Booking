using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Commands
{
    public class CreateAppointmentCommand : IRequest<Guid>
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }

        public string Title { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string? Notes { get; set; }
    }
}
