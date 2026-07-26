using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Identity.Models
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
