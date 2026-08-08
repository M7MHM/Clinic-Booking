using AutoMapper;
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

        public GetAllDoctorQueryHandler(IMapper mapper, IDoctorRepo doctorRepo)
        {
            _mapper = mapper;
            _doctorRepo = doctorRepo;
        }
        public async Task<List<DoctorDto>> Handle(GetAllDoctorQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepo.AllDoctorsAsync();
           return _mapper.Map<List<DoctorDto>>(doctors);
        }
    }
}
