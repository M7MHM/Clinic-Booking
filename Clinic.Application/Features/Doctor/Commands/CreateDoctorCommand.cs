using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Doctor.Commands
{
    public record CreateDoctorCommand(
        string Name,
        int Age,
        string Specialization,
        string Email
        ) :IRequest<Guid>;

}
