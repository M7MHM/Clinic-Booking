using AutoMapper;
using Clinic.Application.Common.Interfaces;
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
        private readonly ICacheService _cacheService;
        public GetPatientByIdQueryHandler(IMapper mapper , IPatientRepo patientRepo , ICacheService cacheService)
        {
            _mapper = mapper;
            _patientRepo = patientRepo;
            _cacheService = cacheService;
        }
        public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"patients:{request.id}";

            var cachedPatient = await _cacheService.GetAsync<PatientDto>(cacheKey);
            if (cachedPatient != null)
                return cachedPatient;

            var patient = await _patientRepo.GetPatientByIdAsync(request.id);
            if (patient == null)
                return null;
            var patientDto = _mapper.Map<PatientDto>(patient);    

            await _cacheService.SetAsync(cacheKey, patientDto, TimeSpan.FromMinutes(5));

            return patientDto;
        }
    }
}
