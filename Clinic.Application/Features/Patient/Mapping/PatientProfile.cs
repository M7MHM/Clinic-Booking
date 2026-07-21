using AutoMapper;
using Clinic.Application.Features.Patient.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Patient.Mapping
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<Domain.Tables.Patient , PatientDto>();
        }
    }
}
