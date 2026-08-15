using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Doctor.Dtos;
using Clinic.Application.Features.Doctor.Queries;
using Clinic.Domain.interfaces.repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Doctor.Handlers
{
    public class GetAllDoctorQueryHandler : IRequestHandler<GetAllDoctorQuery, List<DoctorDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDoctorRepo _doctorRepo;
        private readonly ICacheService _cacheService;
        private const string CacheKey = "doctors:all";
        public GetAllDoctorQueryHandler(IMapper mapper, IDoctorRepo doctorRepo , ICacheService cacheService)
        {
            _mapper = mapper;
            _doctorRepo = doctorRepo;
            _cacheService = cacheService;
        }
        public async Task<List<DoctorDto>> Handle(GetAllDoctorQuery request, CancellationToken cancellationToken)
        {
            var cachedDoctors = await _cacheService.GetAsync<List<DoctorDto>>(CacheKey);
            if (cachedDoctors != null)
                return cachedDoctors;
            
            var doctors = await _doctorRepo.AllDoctorsAsync();
            var doctorDtos = _mapper.Map<List<DoctorDto>>(doctors);

            await _cacheService.SetAsync(CacheKey, doctorDtos, TimeSpan.FromMinutes(5));

            return doctorDtos;
        }
    }
}
