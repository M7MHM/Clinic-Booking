using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Doctor.Dtos;
using Clinic.Application.Features.Doctor.Queries;
using Clinic.Domain.interfaces.repos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Doctor.Handlers
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto>
    {
        private readonly IDoctorRepo _doctorRepo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;


        public GetDoctorByIdQueryHandler(IDoctorRepo doctorRepo, IMapper mapper , ICacheService cacheService)
        {
            _doctorRepo = doctorRepo;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"doctors:{request.Id}";

            var cachedDoctor = await _cacheService.GetAsync<DoctorDto>(cacheKey);
            if (cachedDoctor != null)
                return cachedDoctor;

            var doctor = await _doctorRepo.GetDoctorByIdAsync(request.Id);
            if (doctor == null)
                return null;

            var doctorDto = _mapper.Map<DoctorDto>(doctor);

            await _cacheService.SetAsync(cacheKey, doctorDto, TimeSpan.FromMinutes(5));

            return doctorDto;
        }
    }
}