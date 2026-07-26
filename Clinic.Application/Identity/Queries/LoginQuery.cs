using Clinic.Application.Identity.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Identity.Queries
{
    public record LoginQuery(
        string Email,
        string Password) : IRequest<AuthResult>;
}
