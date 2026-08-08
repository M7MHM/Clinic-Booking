using Clinic.Api.Controllers;
using Clinic.Application.Features.Patient.Commands;
using Clinic.Application.Features.Patient.Dtos;
using Clinic.Application.Features.Patient.Queries;
using Clinic.Application.Features.Patient.Queries.Clinic.Application.Patients.Queries;
using Clinic.Application.Patients.Commands;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Clinic.Application.UnitTests.Controllers
{
    public class PatientsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PatientsController _controller;

        public PatientsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PatientsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllPatient_Should_ReturnOkResultWithListOfPatientDtos()
        {
            var expectedList = new List<PatientDto>
            {
                new PatientDto { Id = Guid.NewGuid(), Name = "mahmoud mohamed" },
                new PatientDto { Id = Guid.NewGuid(), Name = "Mohamed Salah" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllPatientsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedList);

            var result = await _controller.GetAllPatients();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task GetPatientId_Should_ReturnOkResultWithPatientDto_WhenPatientExists()
        {
            var patientId = Guid.NewGuid();
            var expectedDto = new PatientDto { Id = patientId, Name = "Ahmed Ali" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetPatientByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDto);

            var result = await _controller.GetPatientId(patientId);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task GetPatientId_Should_ReturnNotFound_WhenPatientDoesNotExist()
        {
            var patientId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetPatientByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PatientDto?)null);

            var result = await _controller.GetPatientId(patientId);

            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task AddPatient_Should_ReturnOkResult_WhenCommandIsValid()
        {
            var command = new CreatePatientCommand("mahmoud mohamed", 22 , "mahmoud@clinic.com");
            var expectedId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreatePatientCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedId);

            var result = await _controller.AddPatient(command);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdatePatient_Should_ReturnNoContent_WhenIdMatchesCommandId()
        {
            var patientId = Guid.NewGuid();
            var command = new UpdatePatientCommand(patientId, "Updated Name");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdatePatientCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdatePatient(patientId, command);

            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204); 
        }
    }
}