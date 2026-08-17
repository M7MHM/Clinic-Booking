using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Clinic.Application.Features.Appointment.Commands
{
    public record CancelAppointmentCommand(Guid AppointmentId) : IRequest;
}