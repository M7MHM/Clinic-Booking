using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Commands
{
    public record CreateAppointmentCommand (
        Guid DoctorId,
         Guid PatientId,
         string Title ,
         DateTime AppointmentDate ,
         string? Notes 
    ) : IRequest<Guid>;
}
