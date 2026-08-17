using Clinic.Application.Common.Interfaces;
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
    public class AddDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Guid>
    {
        private readonly IDoctorRepo _doctorRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        public AddDoctorCommandHandler(IDoctorRepo doctorRepo , IUnitOfWork unitOfWork , ICacheService cacheService)
        {
            _doctorRepo = doctorRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
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

            await _cacheService.RemoveAsync("doctors:all");
            
            return doctor.Id;
        }
    }
}
