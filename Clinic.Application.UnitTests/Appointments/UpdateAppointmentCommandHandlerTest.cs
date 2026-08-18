using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Appointment.Commands;
using Clinic.Application.Features.Appointment.Handlers;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clinic.Application.UnitTests.Appointments
{
    public class UpdateAppointmentCommandHandlerTest
    {
        private readonly Mock<IAppointmentRepo> _appointmentRepo;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly UpdateAppointmentCommandHandler _handler;
        private readonly Mock<ICacheService> _cacheServiceMock;
        public UpdateAppointmentCommandHandlerTest()
        {
            _appointmentRepo = new Mock<IAppointmentRepo>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _cacheServiceMock = new Mock<ICacheService>();
            _handler = new UpdateAppointmentCommandHandler(_appointmentRepo.Object, _unitOfWork.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateAppointmentAndSaveChanges_WhenAppointmentExists()
        {
            // Arrange
            var appointment = new Appointment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Old Title",
                DateTime.UtcNow,
                "Old Notes");

            var command = new UpdateAppointmentCommand(
                appointment.Id,
                "Updated Title",
                DateTime.UtcNow.AddDays(2),
                "Updated Notes");

            _appointmentRepo.Setup(repo => repo.GetAppointmentByIdAsync(command.Id))
                .ReturnsAsync(appointment);

            _appointmentRepo.Setup(repo => repo.UpdateAppointmentAsync(appointment))
                .Returns(Task.CompletedTask);

            _unitOfWork
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            appointment.Title.Should().Be(command.Title);
            appointment.AppointmentDate.Should().Be(command.AppointmentDate);
            appointment.Notes.Should().Be(command.Notes);

            _appointmentRepo.Verify(repo => repo.GetAppointmentByIdAsync(command.Id),Times.Once);

            _appointmentRepo.Verify(repo => repo.UpdateAppointmentAsync(appointment),Times.Once);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task Handle_Should_NotUpdateOrSaveChanges_WhenAppointmentDoesNotExist()
        {
            var appointmentId = Guid.NewGuid();

            var command = new UpdateAppointmentCommand(
                appointmentId,
                "Updated Title",
                DateTime.UtcNow.AddDays(2),
                "Updated Notes");

            _appointmentRepo.Setup(repo => repo.GetAppointmentByIdAsync(appointmentId))
                .ReturnsAsync((Appointment?)null);

            await _handler.Handle(command, CancellationToken.None);

            _appointmentRepo.Verify(repo => repo.GetAppointmentByIdAsync(appointmentId),Times.Once);

            _appointmentRepo.Verify(repo => repo.UpdateAppointmentAsync(It.IsAny<Appointment>()),Times.Never);

            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Never);
        }
    }
}