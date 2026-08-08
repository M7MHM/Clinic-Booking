using AutoMapper;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Application.Features.Appointment.Handlers;
using Clinic.Application.Features.Appointment.Queries;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clinic.Application.UnitTests.Appointments
{
    public class GetAppointmentsByDoctorIdQueryHandlerTest
    {
        private readonly Mock<IAppointmentRepo> _appointmentRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAppointmentsByDoctorIdQueryHandler _handler;

        public GetAppointmentsByDoctorIdQueryHandlerTest()
        {
            _appointmentRepoMock = new Mock<IAppointmentRepo>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetAppointmentsByDoctorIdQueryHandler(
                _appointmentRepoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnMappedAppointments_WhenDoctorExists()
        {
            var doctorId = Guid.NewGuid();
            var appointments = new List<Appointment>
            {
                new Appointment(
                    doctorId,
                    Guid.NewGuid(),
                    "Dental Check",
                    DateTime.UtcNow,
                    "First appointment"),

                new Appointment(
                    doctorId,
                    Guid.NewGuid(),
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
                    Notes = appointments[0].Notes ?? string.Empty
                },

                new AppointmentDto
                {
                    Id = appointments[1].Id,
                    Title = appointments[1].Title,
                    AppointmentDate = appointments[1].AppointmentDate,
                    Notes = appointments[1].Notes ?? string.Empty
                }
            };

            _appointmentRepoMock.Setup(repo => repo.GetAppointmentByDoctorIdAsync(doctorId))
                .ReturnsAsync(appointments);

            _mapperMock.Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);

            var query = new GetAppointmentsByDoctorIdQuery(doctorId);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(expectedDtos);

            _appointmentRepoMock.Verify(repo => repo.GetAppointmentByDoctorIdAsync(doctorId),Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<List<AppointmentDto>>(appointments),Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_WhenDoctorHasNoAppointments()
        {
            var doctorId = Guid.NewGuid();
            var appointments = new List<Appointment>();
            var expectedDtos = new List<AppointmentDto>();

            _appointmentRepoMock.Setup(repo => repo.GetAppointmentByDoctorIdAsync(doctorId))
                .ReturnsAsync(appointments);

            _mapperMock.Setup(mapper => mapper.Map<List<AppointmentDto>>(appointments))
                .Returns(expectedDtos);

            var query = new GetAppointmentsByDoctorIdQuery(doctorId);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeEmpty();
            _appointmentRepoMock.Verify(repo => repo.GetAppointmentByDoctorIdAsync(doctorId),Times.Once);
        }
    }
}