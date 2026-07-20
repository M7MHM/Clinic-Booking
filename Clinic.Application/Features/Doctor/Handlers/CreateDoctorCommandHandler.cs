using Clinic.Application.Features.Doctor.Commands;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Doctor.Handlers
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Guid>
    {
        private readonly IDoctorRepo _doctorRepo;
        private readonly IUnitOfWork _unitOfWork;
        public CreateDoctorCommandHandler(IDoctorRepo doctorRepo , IUnitOfWork unitOfWork)
        {
            _doctorRepo = doctorRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = new Domain.Tables.Doctor(
                request.Name,
                request.Age,
                request.Specialization,
                request.Email
            );
            await _doctorRepo.AddDoctorAsync(doctor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return doctor.Id;
        }
    }
}
