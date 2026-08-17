using AutoMapper;
using Clinic.Application.Common.Interfaces;
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
        private readonly ICacheService _cacheService;
        private const string CacheKey = "patients:all";

        public GetAllPatientsQueryHandler(IMapper mapper, IPatientRepo patientRepo , ICacheService cacheService)
        {
            _mapper = mapper;
            _patientRepo = patientRepo;
            _cacheService = cacheService;
        }

        public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var cachedPatient = await _cacheService.GetAsync<List<PatientDto>>(CacheKey);
            if(cachedPatient != null)
                return cachedPatient;
            var patients = await _patientRepo.AllPatientAsync();
            var patientDto = _mapper.Map<List<PatientDto>>(patients);

            await _cacheService.SetAsync(CacheKey, patientDto, TimeSpan.FromMinutes(5));

            return patientDto;
        }
    }
}