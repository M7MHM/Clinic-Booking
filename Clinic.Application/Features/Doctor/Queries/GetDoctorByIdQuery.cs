using Clinic.Application.Features.Doctor.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Doctor.Queries
{
    public record GetDoctorByIdQuery(Guid Id):IRequest<DoctorDto>;
}
