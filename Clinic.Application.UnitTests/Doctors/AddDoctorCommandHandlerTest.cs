using Clinic.Application.Features.Doctor.Commands;
using Clinic.Application.Features.Doctor.Handlers;
using Clinic.Application.Features.Patient.Commands;
using Clinic.Application.Features.Patient.Handlers;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.UnitTests.Doctors
{
    public class AddDoctorCommandHandlerTest
    {
        private readonly Mock<IDoctorRepo> _doctorRepo;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly AddDoctorCommandHandler _handler;
        public AddDoctorCommandHandlerTest()
        {
            _doctorRepo = new Mock<IDoctorRepo>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new AddDoctorCommandHandler(_doctorRepo.Object, _unitOfWork.Object);
        }
        [Fact]
        public async Task Handle_Should_CallRepoAndUnitOfWork_WhenDoctorIsValid()
        {
            var doctor = new CreateDoctorCommand("mahmoud", 31, "dentist","mahmoud@clinc.com");

            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(doctor, CancellationToken.None);

            _doctorRepo.Verify(p => p.AddDoctorAsync(It.IsAny<Doctor>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
