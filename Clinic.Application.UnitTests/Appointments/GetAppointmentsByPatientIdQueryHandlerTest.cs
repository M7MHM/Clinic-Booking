using AutoMapper;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Application.Features.Appointment.Handlers;
using Clinic.Application.Features.Appointment.Queries;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using FluentAssertions;
using Moq;

namespace Clinic.Application.UnitTests.Appointments
{
    public class GetAppointmentsByPatientIdQueryHandlerTest
    {
        private readonly Mock<IAppointmentRepo> _appointmentRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly GetAppointmentsByPatientIdQueryHandler _handler;

        public GetAppointmentsByPatientIdQueryHandlerTest()
        {
            _appointmentRepo = new Mock<IAppointmentRepo>();
            _mapper = new Mock<IMapper>();

            _handler = new GetAppointmentsByPatientIdQueryHandler(
                _appointmentRepo.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnMappedAppointments_WhenPatientHasAppointments()
        {
            var patientId = Guid.NewGuid();
            var appointments = new List<Appointment>
            {
                new Appointment(
                    Guid.NewGuid(),
                    patientId,
                    "Dental Check",
                    DateTime.UtcNow,
                    "First appointment"),

                new Appointment(
                    Guid.NewGuid(),
                    patientId,
                    "Follow Up",
                    DateTime.UtcNow.AddDays(1),
                    "Follow up appointment")
            };

            var expectedDtos = new List<AppointmentDto>
            {
                new AppointmentDto
                {
                    Id = appointments[0].Id,
                    Title = appointments[0].Title,
                    AppointmentDate = appointments[0].AppointmentDate,
                    Notes = appointments[0].Notes ?? string.Empty,
                    Status = appointments[0].Status
                },

                new AppointmentDto
                {
                    Id = appointments[1].Id,
                    Title = appointments[1].Title,
                    AppointmentDate = appointments[1].AppointmentDate,
                    Notes = appointments[1].Notes ?? string.Empty,
                    Status = appointments[1].Status
                }
            };

            _appointmentRepo.Setup(repo => repo.GetAppointmentByPatientIdAsync(patientId))
                .ReturnsAsync(appointments);

            _mapper.Setup(m => m.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);

            var query = new GetAppointmentsByPatientQuery(patientId);

            var result = await _handler.Handle(query,CancellationToken.None);

            result.Should().BeEquivalentTo(expectedDtos);
            _appointmentRepo.Verify(repo => repo.GetAppointmentByPatientIdAsync(patientId),Times.Once);
            _mapper.Verify(m => m.Map<List<AppointmentDto>>(appointments),Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_WhenPatientHasNoAppointments()
        {
            var patientId = Guid.NewGuid();
            var appointments = new List<Appointment>();
            var expectedDtos = new List<AppointmentDto>();

            _appointmentRepo.Setup(repo => repo.GetAppointmentByPatientIdAsync(patientId))
                .ReturnsAsync(appointments);

            _mapper.Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);

            var query = new GetAppointmentsByPatientQuery(patientId);

            var result = await _handler.Handle(query,CancellationToken.None);

            result.Should().BeEmpty();

            _appointmentRepo.Verify(repo => repo.GetAppointmentByPatientIdAsync(patientId),Times.Once);
            _mapper.Verify(mapper => mapper.Map<List<AppointmentDto>>(appointments),Times.Once);
        }
    }
}