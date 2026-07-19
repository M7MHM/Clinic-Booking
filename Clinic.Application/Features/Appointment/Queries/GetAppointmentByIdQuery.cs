using Clinic.Application.Features.Appointment.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Queries
{
    public class GetAppointmentByIdQuery :IRequest<AppointmentDto>
    {
        Guid Id { get; set; }
        public GetAppointmentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
