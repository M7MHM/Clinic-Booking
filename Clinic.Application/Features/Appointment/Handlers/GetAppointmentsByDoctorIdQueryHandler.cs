using AutoMapper;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Application.Features.Appointment.Queries;
using Clinic.Domain.interfaces.repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Handlers
{
    public class GetAppointmentsByDoctorIdQueryHandler : IRequestHandler<GetAppointmentsByDoctorIdQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IMapper _mapper;

        public GetAppointmentsByDoctorIdQueryHandler(IAppointmentRepo appointmentRepo, IMapper mapper)
        {
            _appointmentRepo = appointmentRepo;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepo.GetAppointmentByDoctorIdAsync(request.DoctorId);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
    } 
}
