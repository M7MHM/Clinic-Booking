using Clinic.Application.Identity.Commands;
using Clinic.Application.Identity.Interfaces;
using Clinic.Application.Identity.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
namespace Clinic.Application.UnitTests.Auth
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IAuthService> _authService;
        private readonly RegisterCommandHandler _register;
        public RegisterCommandHandlerTests()
        {
            _authService = new Mock<IAuthService>();
            _register = new RegisterCommandHandler(_authService.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenNewUserIsRegistered()
        {
            var command = new RegisterCommand("mahmoud", "ayad", "mahmoud@clinc", "mahmoud#7" , "doctor");
            var expectedResult = new AuthResult { IsAuthenticated = true , Message = "Success"};

            _authService
                .Setup(r => r.RegisterAsync(command.FirstName, command.LastName, command.Email, command.Password, command.UserType))
                .ReturnsAsync(expectedResult);

            var result = await _register.Handle(command , CancellationToken.None);

            result.Should().NotBeNull();
            result.IsAuthenticated.Should().BeTrue();
            _authService.Verify(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
