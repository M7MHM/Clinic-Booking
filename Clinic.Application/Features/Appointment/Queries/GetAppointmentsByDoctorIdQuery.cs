using Clinic.Application.Features.Appointment.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Queries
{
    public record GetAppointmentsByDoctorIdQuery(Guid DoctorId) :IRequest<List<AppointmentDto>>;
}
