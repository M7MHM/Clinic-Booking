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
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IPatientRepo _patientRepo;
        private readonly IUnitOfWork _unitOfWork;
        public CreatePatientCommandHandler(IPatientRepo patientRepo ,IUnitOfWork unitOfWork)
        {
            _patientRepo = patientRepo;
            _unitOfWork = unitOfWork;
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
            return patient.Id;
        }
    }
}
