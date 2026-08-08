using Clinic.Api.Controllers;
using Clinic.Application.Features.Doctor.Commands;
using Clinic.Application.Features.Doctor.Dtos;
using Clinic.Application.Features.Doctor.Handlers;
using Clinic.Application.Features.Doctor.Queries;
using Clinic.Application.Features.Patient.Commands;
using Clinic.Application.Features.Patient.Dtos;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.UnitTests.Controllers
{
    public class DoctorsControllerTests
    {

        private readonly Mock<IMediator> _mediatorMock;
        private readonly DoctorsController _controller;

        public DoctorsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new DoctorsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllDoctors_Should_ReturnOkResultWithListOfDoctorDtos()
        {
            var expectedList = new List<DoctorDto>
            {
                new DoctorDto { Id = Guid.NewGuid(), Name = "mahmoud mohamed" },
                new DoctorDto { Id = Guid.NewGuid(), Name = "Mohamed Salah" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllDoctorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedList);

            var result = await _controller.GetAllDoctors();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task GetDoctorId_Should_ReturnOkResultWithDoctorDto_WhenDoctorExists()
        {
            var doctorId = Guid.NewGuid();
            var expectedDto = new DoctorDto { Id = doctorId, Name = "Ahmed Ali" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetDoctorByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDto);

            var result = await _controller.GetDoctorId(doctorId);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task GetDoctorId_Should_ReturnNotFound_WhenDoctorDoesNotExist()
        {
            var doctorId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetDoctorByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DoctorDto?)null);

            var result = await _controller.GetDoctorId(doctorId);

            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task AddDoctor_Should_ReturnOkResult_WhenCommandIsValid()
        {
            var command = new CreateDoctorCommand("mahmoud mohamed", 22,"dentist", "mahmoud@clinic.com");
            var expectedId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateDoctorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedId);

            var result = await _controller.AddDoctor(command);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().Be(expectedId);

            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDoctor_Should_ReturnNoContent_WhenIdMatchesCommandId()
        {
            var doctorId = Guid.NewGuid();
            var command = new UpdateDoctorCommand(doctorId, "Updated Name" , "Update Specialization");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateDoctorCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateDoctor(doctorId, command);

            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
