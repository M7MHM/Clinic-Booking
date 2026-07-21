using AutoMapper;
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

        public GetDoctorByIdQueryHandler(IDoctorRepo doctorRepo, IMapper mapper)
        {
            _doctorRepo = doctorRepo;
            _mapper = mapper;
        }

        public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepo.GetDoctorByIdAsync(request.Id);
            if (doctor == null) throw new Exception($"Doctor with Id {request.Id} was not found.");
            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}