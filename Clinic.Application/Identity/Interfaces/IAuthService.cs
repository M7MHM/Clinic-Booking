using Clinic.Application.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Identity.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(string firstName, string lastName, string email, string password , string userType); 
        Task<AuthResult> LoginAsync (string email, string password);
    }
}
