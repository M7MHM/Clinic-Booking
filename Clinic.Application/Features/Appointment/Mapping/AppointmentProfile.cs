using AutoMapper;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Domain.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Mapping
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
             CreateMap<Clinic.Domain.Tables.Appointment, AppointmentDto>();
        }
    }
}
