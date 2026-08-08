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

namespace Clinic.Application.UnitTests.Patients
{
    public class AddPatientCommandHandlerTest
    {
        private readonly Mock<IPatientRepo> _patientRepo;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly AddPatientCommandHandler _handler;
        public AddPatientCommandHandlerTest()
        {
            _patientRepo = new Mock<IPatientRepo>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new AddPatientCommandHandler(_patientRepo.Object, _unitOfWork.Object);
        }
        [Fact]
        public async Task Handle_Should_CallRepoAndUnitOfWork_WhenPatientIsValid()
        {
            var patient = new CreatePatientCommand("mahmoud", 22, "mahmoud@clinc.com");

            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _handler.Handle(patient, CancellationToken.None);

            _patientRepo.Verify(p => p.AddPatientAsync(It.IsAny<Patient>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
