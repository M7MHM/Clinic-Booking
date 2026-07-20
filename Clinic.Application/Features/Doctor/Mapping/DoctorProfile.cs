using AutoMapper;
using Clinic.Application.Features.Doctor.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Doctor.Mapping
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<Domain.Tables.Doctor, DoctorDto>();
        }
    }
}
