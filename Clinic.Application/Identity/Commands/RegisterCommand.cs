using Clinic.Application.Identity.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Identity.Commands
{
    public record RegisterCommand(
        string FirstName,
    string LastName,
    string Email,
    string Password,
    string UserType) : IRequest<AuthResult>;
}
