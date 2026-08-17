using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Patient.Commands;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using MediatR;

namespace Clinic.Application.Patients.Commands
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
    {
        private readonly IPatientRepo _patientRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        public UpdatePatientCommandHandler(IPatientRepo patientRepo, IUnitOfWork unitOfWork , ICacheService cacheService)
        {
            _patientRepo = patientRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepo.GetPatientByIdAsync(request.Id);
            if (patient == null) return;

            patient.Name = request.Name; 
            await _patientRepo.UpdatePatientAsync(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync("patients:all");
            await _cacheService.RemoveAsync($"patients:{request.Id}");
        }
    }
}