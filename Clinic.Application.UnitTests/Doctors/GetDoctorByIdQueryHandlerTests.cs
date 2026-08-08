using AutoMapper;
using Clinic.Application.Features.Doctor.Dtos;
using Clinic.Application.Features.Doctor.Handlers;
using Clinic.Application.Features.Doctor.Queries;
using Clinic.Application.Features.Patient.Dtos;
using Clinic.Application.Features.Patient.Handlers;
using Clinic.Application.Features.Patient.Queries;
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
    public class GetDoctorByIdQueryHandlerTests
    {

        private readonly Mock<IDoctorRepo> _doctorRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetDoctorByIdQueryHandler _handler;

        public GetDoctorByIdQueryHandlerTests()
        {
            _doctorRepoMock = new Mock<IDoctorRepo>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetDoctorByIdQueryHandler(
                _doctorRepoMock.Object, _mapperMock.Object
            );
        }
        [Fact]
        public async Task Handle_Should_ReturnDoctorDto_WhenDoctorExists()
        {
            var doctorId = Guid.NewGuid();
            var expectedResult = new Doctor { Id = doctorId, Name = "mahmoud mohamed" };
            var expectedDto = new DoctorDto { Id = doctorId, Name = "mahmoud mohamed" };

            _doctorRepoMock.Setup(repo => repo.GetDoctorByIdAsync(doctorId))
                .ReturnsAsync(expectedResult);
            _mapperMock.Setup(m => m.Map<DoctorDto>(It.IsAny<Doctor>()))
                .Returns(expectedDto);

            var query = new GetDoctorByIdQuery(doctorId);
            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(doctorId);
            result.Name.Should().Be("mahmoud mohamed");

            _doctorRepoMock.Verify(repo => repo.GetDoctorByIdAsync(doctorId), Times.Once());
        }
        [Fact]
        public async Task Handle_Should_ThrowException_WhenDoctorDoesNotExist()
        {
            var doctorId = Guid.NewGuid();
            var query = new GetDoctorByIdQuery(doctorId);

            _doctorRepoMock.Setup(r => r.GetDoctorByIdAsync(doctorId))
                .ReturnsAsync((Doctor?)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeNull();
            _doctorRepoMock.Verify(r => r.GetDoctorByIdAsync(doctorId), Times.Once);
        }
    }
}
