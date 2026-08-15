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
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand>
    {
        private readonly IDoctorRepo _doctorRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        public UpdateDoctorCommandHandler(IDoctorRepo doctorRepo, IUnitOfWork unitOfWork , ICacheService cacheService)
        {
             _doctorRepo = doctorRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }
        public async Task Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepo.GetDoctorByIdAsync(request.Id);
            if (doctor == null)
                return;

            doctor.Name = request.Name;
            doctor.Specialization = request.Specialization;

            await _doctorRepo.UpdateDoctorAsync(doctor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync("doctors:all");
            await _cacheService.RemoveAsync($"doctors:{request.Id}");
        }
    }
}
