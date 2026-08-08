using AutoMapper;
using Clinic.Application.Features.Patient.Dtos;
using Clinic.Application.Features.Patient.Queries.Clinic.Application.Patients.Queries;
using Clinic.Domain.interfaces.repos;
using MediatR;

namespace Clinic.Application.Features.Patient.Handlers
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientDto>>
    {
        private readonly IMapper _mapper;
        private readonly IPatientRepo _patientRepo;

        public GetAllPatientsQueryHandler(IMapper mapper, IPatientRepo patientRepo)
        {
            _mapper = mapper;
            _patientRepo = patientRepo;
        }

        public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepo.AllPatientAsync();
            return _mapper.Map<List<PatientDto>>(patients);
        }
    }
}