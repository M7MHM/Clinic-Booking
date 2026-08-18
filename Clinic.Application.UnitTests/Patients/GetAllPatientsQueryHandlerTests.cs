using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Patient.Dtos;
using Clinic.Application.Features.Patient.Handlers;
using Clinic.Application.Features.Patient.Queries.Clinic.Application.Patients.Queries;
using Clinic.Domain.interfaces.repos;
using Clinic.Domain.Tables;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clinic.Application.UnitTests.Patients
{
    public class GetAllPatientsQueryHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPatientRepo> _patientRepoMock;
        private readonly GetAllPatientsQueryHandler _handler;
        private readonly Mock<ICacheService> _cacheServiceMock;
        public GetAllPatientsQueryHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _patientRepoMock = new Mock<IPatientRepo>();
            _cacheServiceMock = new Mock<ICacheService>();
            _handler = new GetAllPatientsQueryHandler(_mapperMock.Object, _patientRepoMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnListOfPatientDtos_WhenPatientsExist()
        {
            var patients = new List<Patient>
            {
                new Patient { Id = Guid.NewGuid(), Name = "mahmoud mohamed" },
                new Patient { Id = Guid.NewGuid(), Name = "Mohamed Salah" }
            };

            var expectedDtos = new List<PatientDto>
            {
                new PatientDto { Id = patients[0].Id, Name = "mahmoud mohamed" },
                new PatientDto { Id = patients[1].Id, Name = "Mohamed Salah" }
            };

            _patientRepoMock.Setup(repo => repo.AllPatientAsync())
                            .ReturnsAsync(patients);
            _mapperMock.Setup(m => m.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
                       .Returns(expectedDtos);

            var query = new GetAllPatientsQuery();
            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedDtos);
            _patientRepoMock.Verify(repo => repo.AllPatientAsync(), Times.Once);
        }
    }
}