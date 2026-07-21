using AutoMapper;
using Clinic.Application.Features.Patient.Dtos;
using Clinic.Application.Features.Patient.Queries;
using Clinic.Domain.interfaces.repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Patient.Handlers
{
    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDto>
    {
        private readonly IPatientRepo _patientRepo;
        private readonly IMapper _mapper;
        public GetPatientByIdQueryHandler(IMapper mapper , IPatientRepo patientRepo)
        {
            _mapper = mapper;
            _patientRepo = patientRepo;
        }
        public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepo.GetPatientByIdAsync(request.id);
            if (patient == null)
                throw new Exception($"Appointment with Id {request.id} was not found.");
            return _mapper.Map<PatientDto>(patient);    
        }
    }
}
