using AutoMapper;
using Clinic.Application.Common.Interfaces;
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

namespace Clinic.Application.UnitTests.Patients
{
    public class GetPatientByIdQueryHandlerTests
    {
        private readonly Mock<IPatientRepo> _patientRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetPatientByIdQueryHandler _handler;
        private readonly Mock<ICacheService> _cacheServiceMock;
        public GetPatientByIdQueryHandlerTests()
       {
            _patientRepoMock = new Mock<IPatientRepo>();
            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<ICacheService>();
            _handler = new GetPatientByIdQueryHandler(_mapperMock.Object,_patientRepoMock.Object,_cacheServiceMock.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnPatientDto_WhenPatientExists()
        {
            var patientId = Guid.NewGuid();
            var expectedResult = new Patient { Id = patientId, Name = "mahmoud mohamed" };
            var expectedDto = new PatientDto { Id = patientId, Name = "mahmoud mohamed" };

            _patientRepoMock.Setup(repo => repo.GetPatientByIdAsync(patientId))
                .ReturnsAsync(expectedResult);
            _mapperMock.Setup(m => m.Map<PatientDto>(It.IsAny<Patient>()))
                .Returns(expectedDto);

            var query = new GetPatientByIdQuery(patientId);
            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(patientId);
            result.Name.Should().Be("mahmoud mohamed");

            _patientRepoMock.Verify(repo => repo.GetPatientByIdAsync(patientId), Times.Once());
        }
        [Fact]
        public async Task Handle_Should_ThrowException_WhenPatientDoesNotExist()
        {
            var patientId = Guid.NewGuid();
            var query = new GetPatientByIdQuery(patientId);

            _patientRepoMock.Setup(r => r.GetPatientByIdAsync(patientId))
                            .ReturnsAsync((Patient?)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeNull();
            _patientRepoMock.Verify(r => r.GetPatientByIdAsync(patientId), Times.Once);
        }
    }
}
