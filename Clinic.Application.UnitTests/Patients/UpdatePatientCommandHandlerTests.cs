using Clinic.Application.Features.Patient.Commands;
using Clinic.Application.Patients.Commands;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clinic.Application.UnitTests.Patients
{
    public class UpdatePatientCommandHandlerTests
    {
        private readonly Mock<IPatientRepo> _patientRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdatePatientCommandHandler _handler;

        public UpdatePatientCommandHandlerTests()
        {
            _patientRepoMock = new Mock<IPatientRepo>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new UpdatePatientCommandHandler(_patientRepoMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdatePatientAndSaveChanges_WhenPatientExists()
        {
            var patientId = Guid.NewGuid();
            var existingPatient = new Patient { Id = patientId, Name = "Old Name" };
            var command = new UpdatePatientCommand(patientId, "New Name");

            _patientRepoMock.Setup(r => r.GetPatientByIdAsync(patientId))
                            .ReturnsAsync(existingPatient);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            existingPatient.Name.Should().Be("New Name");
            _patientRepoMock.Verify(r => r.UpdatePatientAsync(existingPatient), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_DoNothing_WhenPatientDoesNotExist()
        {
            var patientId = Guid.NewGuid();
            var command = new UpdatePatientCommand(patientId, "New Name");

            _patientRepoMock.Setup(r => r.GetPatientByIdAsync(patientId))
                            .ReturnsAsync((Patient?)null);

            await _handler.Handle(command, CancellationToken.None);

            _patientRepoMock.Verify(r => r.UpdatePatientAsync(It.IsAny<Patient>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}