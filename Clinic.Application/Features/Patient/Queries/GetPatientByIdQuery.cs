using Clinic.Application.Features.Patient.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Patient.Queries
{
    public record GetPatientByIdQuery(Guid id):IRequest<PatientDto>;
}
