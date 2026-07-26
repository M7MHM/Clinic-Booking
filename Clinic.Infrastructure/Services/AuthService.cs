using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Clinic.Application.Identity.Interfaces;
using Clinic.Application.Identity.Models;
using Clinic.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Clinic.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthResult
                {
                    IsAuthenticated = false,
                    Message = "Invalid Email or Password."
                };
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!checkPassword)
            {
                return new AuthResult
                {
                    IsAuthenticated = false,
                    Message = "Invalid Email or Password."
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = GenerateJwtToken(user, roles);

            return new AuthResult
            {
                IsAuthenticated = true,
                Message = "Login Successful",
                Token = token
            };
        }

        public async Task<AuthResult> RegisterAsync(string firstName, string lastName, string email, string password, string userType)
        {
            var emailExist = await _userManager.FindByEmailAsync(email);
            if (emailExist != null)
            {
                return new AuthResult
                {
                    IsAuthenticated = false,
                    Message = "Email already exists."
                };
            }

            var user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FirstName = firstName, 
                LastName = lastName,
                UserType = userType
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new AuthResult
                {
                    IsAuthenticated = false,
                    Message = "Registration failed.",
                    Errors = errors
                };
            }
            if (!string.IsNullOrEmpty(userType))
            {
                await _userManager.AddToRoleAsync(user, userType);
            }

            return new AuthResult
            {
                IsAuthenticated = true,
                Message = "User Registered Successfully!"
            };
        }
        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyString = jwtSettings["Key"] ?? "FallbackSecretKeyThatIsVeryLongAndSecureForLocalDev2026!";
            var key = Encoding.UTF8.GetBytes(keyString);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("UserType", user.UserType ?? "")
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var durationInMinutes = double.Parse(jwtSettings["DurationInMinutes"] ?? "60");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(durationInMinutes),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}