using Clinic.Application.Features.Doctor.Commands;
using Clinic.Application.Features.Doctor.Handlers;
using Clinic.Application.Features.Patient.Commands;
using Clinic.Application.Patients.Commands;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.UnitTests.Doctors
{
    public class UpdateDoctorCommandHandlerTest
    {

        private readonly Mock<IDoctorRepo> _doctorRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateDoctorCommandHandler _handler;

        public UpdateDoctorCommandHandlerTest()
        {
            _doctorRepoMock = new Mock<IDoctorRepo>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new UpdateDoctorCommandHandler(_doctorRepoMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateDoctorAndSaveChanges_WhenDoctorExists()
        {
            var doctorId = Guid.NewGuid();
            var existingDoctor = new Doctor { Id = doctorId, Name = "Old Name" , Specialization = "Old Specialization" };
            var command = new UpdateDoctorCommand(doctorId, "New Name", "New Specialization");

            _doctorRepoMock.Setup(r => r.GetDoctorByIdAsync(doctorId))
                            .ReturnsAsync(existingDoctor);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(1);

            await _handler.Handle(command, CancellationToken.None);

            existingDoctor.Name.Should().Be("New Name"); 
            existingDoctor.Specialization.Should().Be("New Specialization");
            _doctorRepoMock.Verify(r => r.UpdateDoctorAsync(existingDoctor), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_NotUpdateDoctor_WhenDoctorDoesNotExist()
        {
            var doctorId = Guid.NewGuid();
            var command = new UpdateDoctorCommand(
                doctorId,
                "New Name",
                "New Specialization");

            _doctorRepoMock.Setup(r => r.GetDoctorByIdAsync(doctorId))
                .ReturnsAsync((Doctor?)null);

            await _handler.Handle(command, CancellationToken.None);

            _doctorRepoMock.Verify(r => r.UpdateDoctorAsync(It.IsAny<Doctor>()),Times.Never);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Never);
        }
    }
}
