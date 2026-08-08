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

        public UpdatePatientCommandHandler(IPatientRepo patientRepo, IUnitOfWork unitOfWork)
        {
            _patientRepo = patientRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepo.GetPatientByIdAsync(request.Id);
            if (patient == null) return;

            patient.Name = request.Name; 
            await _patientRepo.UpdatePatientAsync(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}