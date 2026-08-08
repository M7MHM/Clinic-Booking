using Clinic.Application.Features.Appointment.Commands;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Application.Features.Appointment.Handlers;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.UnitTests.Appointments
{
    public class CreateAppointmentCommandHandlerTest
    {
        private readonly Mock<IAppointmentRepo> _appointmentRepo;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly CreateAppointmentCommandHandler _handler;
        public CreateAppointmentCommandHandlerTest()
        {
            _appointmentRepo = new Mock<IAppointmentRepo>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateAppointmentCommandHandler(_unitOfWork.Object , _appointmentRepo.Object);
        }
        [Fact]
        public async Task Handle_Should_CallRepoAndUnitOfWork_WhenDoctorAndPatientIsValid()
        {
            var DoctorId = Guid.NewGuid();
            var PatientId = Guid.NewGuid();
            var appointment = new CreateAppointmentCommand
                (
                    DoctorId,
                    PatientId,
                    "The appointment with dentist",
                    DateTime.UtcNow.AddDays(1),
                    "Regular checkup"
                );
            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(appointment, CancellationToken.None);

            _appointmentRepo.Verify(a => a.AddAppointmentAsync(It.IsAny<Appointment>()),Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
