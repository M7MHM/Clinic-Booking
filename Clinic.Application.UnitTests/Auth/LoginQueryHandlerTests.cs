using Clinic.Application.Identity.Interfaces;
using Clinic.Application.Identity.Models;
using Clinic.Application.Identity.Queries;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.UnitTests.Auth
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IAuthService> _authService;
        private readonly LoginQueryHandler _login;
        public LoginQueryHandlerTests()
        {
            _authService = new Mock<IAuthService>();
            _login = new LoginQueryHandler(_authService.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnToken_WhenCredentialsAreValid()
        {
            var query = new LoginQuery("mahmoud@clinc.com", "mahmoud#7");
            var expectedResult = new AuthResult { IsAuthenticated = true, Token = "eyJhbGciOiJIUz..."};

            _authService
                .Setup(l => l.LoginAsync(query.Email, query.Password))
                .ReturnsAsync(expectedResult);

            var result = await _login.Handle(query, CancellationToken.None);

            result.Token.Should().NotBeNullOrEmpty();
            result.IsAuthenticated.Should().BeTrue();
        }
    }
}
