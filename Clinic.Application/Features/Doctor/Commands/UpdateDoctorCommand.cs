using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Doctor.Commands
{
    public record UpdateDoctorCommand(Guid Id , string Name , string Specialization) : IRequest;
}
