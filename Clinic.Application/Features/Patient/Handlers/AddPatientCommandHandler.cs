using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Patient.Commands;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Patient.Handlers
{
    public class AddPatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IPatientRepo _patientRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public AddPatientCommandHandler(IPatientRepo patientRepo ,IUnitOfWork unitOfWork , ICacheService cacheService)
        {
            _patientRepo = patientRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }
        public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Domain.Tables.Patient(
                request.Name,
                request.Age,
                request.Email
            );

            await _patientRepo.AddPatientAsync(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync("patients:all");

            return patient.Id;
        }
    }
}
