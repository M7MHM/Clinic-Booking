using Clinic.Api.Controllers;
using Clinic.Application.Features.Appointment.Commands;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Application.Features.Appointment.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Clinic.Application.UnitTests.Controllers
{
    public class AppointmentControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AppointmentsController _controller;

        public AppointmentControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AppointmentsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllDoctorAppointments_Should_ReturnOk_WhenAppointmentsExist()
        {
            var doctorId = Guid.NewGuid();

            var expectedAppointments = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Dental Check",
                    AppointmentDate = DateTime.Now,
                    Notes = "Regular checkup",
                    Status = Domain.Enum.AppointmentStatus.Pending
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsByDoctorIdQuery>(),
                 It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAppointments);

            var result = await _controller.GetAllDoctorAppointments(doctorId);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedAppointments);
        }

        [Fact]
        public async Task GetAllDoctorAppointments_Should_ReturnNotFound_WhenAppointmentsDoNotExist()
        {
            var doctorId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsByDoctorIdQuery>(),
                 It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<AppointmentDto>?)null);

            var result = await _controller.GetAllDoctorAppointments(doctorId);

            var notFoundResult = result as NotFoundResult;

            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAllPatientAppointments_Should_ReturnOk_WhenAppointmentsExist()
        {
            var patientId = Guid.NewGuid();

            var expectedAppointments = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Dental Check",
                    AppointmentDate = DateTime.Now,
                    Notes = "Regular checkup",
                    Status = Domain.Enum.AppointmentStatus.Pending
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsByPatientQuery>(),
                 It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAppointments);

            var result = await _controller.GetAllPatientAppointments(patientId);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedAppointments);
        }

        [Fact]
        public async Task GetAllPatientAppointments_Should_ReturnNotFound_WhenAppointmentsDoNotExist()
        {
            var patientId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentsByPatientQuery>(),
                 It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<AppointmentDto>?)null);

            var result = await _controller.GetAllPatientAppointments(patientId);

            var notFoundResult = result as NotFoundResult;

            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAppointmentById_Should_ReturnOk_WhenAppointmentExists()
        {
            var appointmentId = Guid.NewGuid();

            var expectedAppointment = new AppointmentDto
            {
                Id = appointmentId,
                Title = "Dental Check",
                AppointmentDate = DateTime.Now,
                Notes = "Regular checkup",
                Status = Domain.Enum.AppointmentStatus.Pending
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentByIdQuery>(),
                 It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAppointment);

            var result = await _controller.GetAppointmentById(appointmentId);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedAppointment);
        }

        [Fact]
        public async Task GetAppointmentById_Should_ReturnNotFound_WhenAppointmentDoesNotExist()
        {
            var appointmentId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((AppointmentDto?)null);

            var result = await _controller.GetAppointmentById(appointmentId);

            var notFoundResult = result as NotFoundResult;

            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task AddAppointment_Should_ReturnOk_WhenCommandIsValid()
        {
            var command = new CreateAppointmentCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Dental Check",
                DateTime.Now,
                "Regular checkup");

            var expectedId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateAppointmentCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedId);

            var result = await _controller.AddAppointment(command);

            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedId);
        }

        [Fact]
        public async Task UpdateAppointment_Should_ReturnNoContent_WhenIdsMatch()
        {
            var appointmentId = Guid.NewGuid();

            var command = new UpdateAppointmentCommand(
                appointmentId,
                "Updated Appointment",
                DateTime.Now,
                "Updated notes");

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateAppointmentCommand>(),
                 It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateAppointment(appointmentId, command);

            var noContentResult = result as NoContentResult;

            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task UpdateAppointment_Should_ReturnBadRequest_WhenIdsDoNotMatch()
        {
            var routeId = Guid.NewGuid();
            var command = new UpdateAppointmentCommand(
                Guid.NewGuid(),
                "Updated Appointment",
                DateTime.Now,
                "Updated notes");

            var result = await _controller.UpdateAppointment(routeId, command);

            var badRequestResult = result as BadRequestResult;

            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateAppointmentCommand>(),
                    It.IsAny<CancellationToken>()),Times.Never);
        }
    }
}