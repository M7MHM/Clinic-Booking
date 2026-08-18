using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Doctor.Dtos;
using Clinic.Application.Features.Doctor.Handlers;
using Clinic.Application.Features.Doctor.Queries;
using Clinic.Application.Features.Patient.Dtos;
using Clinic.Application.Features.Patient.Handlers;
using Clinic.Application.Features.Patient.Queries.Clinic.Application.Patients.Queries;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.UnitTests.Doctors
{
    public class GetAllDoctorQueryHandlerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDoctorRepo> _doctorRepoMock;
        private readonly GetAllDoctorQueryHandler _handler;
        private readonly Mock<ICacheService> _cacheServiceMock;
        public GetAllDoctorQueryHandlerTests()
        {
            _mapper = new Mock<IMapper>();
            _doctorRepoMock = new Mock<IDoctorRepo>();
            _cacheServiceMock = new Mock<ICacheService>();
            _handler = new GetAllDoctorQueryHandler(_mapper.Object , _doctorRepoMock.Object , _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnListOfDoctorsDtos_WhenDoctorExist()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { Id = Guid.NewGuid(), Name = "mahmoud mohamed" },
                new Doctor { Id = Guid.NewGuid(), Name = "Mohamed Salah" }
            };

            var expectedDtos = new List<DoctorDto>
            {
                new DoctorDto { Id = doctors[0].Id, Name = "mahmoud mohamed" },
                new DoctorDto { Id = doctors[1].Id, Name = "Mohamed Salah" }
            };

            _doctorRepoMock.Setup(repo => repo.AllDoctorsAsync())
                            .ReturnsAsync(doctors);
            _mapper.Setup(m => m.Map<List<DoctorDto>>(It.IsAny<List<Doctor>>()))
                       .Returns(expectedDtos);

            var query = new GetAllDoctorQuery();
            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedDtos);
            _doctorRepoMock.Verify(repo => repo.AllDoctorsAsync(), Times.Once);
        }
    }
}
