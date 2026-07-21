using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Patient.Commands
{
    public record CreatePatientCommand (
      string Name,
      int Age, 
      string  Email 
    ) : IRequest<Guid>;
}
