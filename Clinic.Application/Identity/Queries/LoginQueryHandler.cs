using Clinic.Application.Identity.Interfaces;
using Clinic.Application.Identity.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Identity.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResult>
    {
        private readonly IAuthService _authService;
        public LoginQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<AuthResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(request.Email , request.Password);
        }
    }
}
