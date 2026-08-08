using AutoMapper;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Application.Features.Appointment.Handlers;
using Clinic.Application.Features.Appointment.Queries;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.UnitTests.Appointments
{
    public class GetAppointmentByIdQueryHandlerTest
    {
        private readonly Mock<IAppointmentRepo> _appointmentRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly GetAppointmentByIdQueryHandler _handler;
        public GetAppointmentByIdQueryHandlerTest()
        {
            _appointmentRepo = new Mock<IAppointmentRepo>();
            _mapper = new Mock<IMapper>();
            _handler = new GetAppointmentByIdQueryHandler(_appointmentRepo.Object ,_mapper.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnListOfAppointmentDtos_WhenAppointmentExist()
        {
            var DoctorId = Guid.NewGuid();
            var PatientId = Guid.NewGuid();
            var appointment = new Appointment
            (
                DoctorId,
                PatientId,
                "The appointment with dentist",
                DateTime.UtcNow.AddDays(1),
                "Regular checkup"
            );
            var expectedDto = new AppointmentDto
            {
                Id = appointment.Id,
                Title = appointment.Title,
                AppointmentDate = appointment.AppointmentDate,
                Notes = appointment.Notes ?? string.Empty,
                DoctorName = "Ahmed Ali",
                PatientName = "Mahmoud Mohamed",
                Status = appointment.Status

            };

            _appointmentRepo.Setup(a => a.GetAppointmentByIdAsync(appointment.Id))
                .ReturnsAsync(appointment);
            _mapper.Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(expectedDto);

            var result = await _handler.Handle(new GetAppointmentByIdQuery(appointment.Id), (CancellationToken.None));

            result.Should().BeEquivalentTo(expectedDto);

            _appointmentRepo.Verify(r => r.GetAppointmentByIdAsync(appointment.Id),Times.Once);

            _mapper.Verify(m => m.Map<AppointmentDto>(appointment),Times.Once);
        }
        [Fact]
        public async Task Handle_Should_ReturnNull_WhenAppointmentDoesNotExist()
        {
            var appointmentId = Guid.NewGuid();

            _appointmentRepo.Setup(r => r.GetAppointmentByIdAsync(appointmentId))
                .ReturnsAsync((Appointment?)null);

            var result = await _handler.Handle(new GetAppointmentByIdQuery(appointmentId), CancellationToken.None);

            result.Should().BeNull();
            _appointmentRepo.Verify(r => r.GetAppointmentByIdAsync(appointmentId), Times.Once);
            _mapper.Verify(m => m.Map<AppointmentDto>(It.IsAny<Appointment>()),Times.Never);
        }
    }
}
