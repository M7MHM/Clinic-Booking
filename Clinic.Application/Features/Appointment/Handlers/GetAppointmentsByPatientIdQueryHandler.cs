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
    public class GetAppointmentsByPatientIdQueryHandler : IRequestHandler<GetAppointmentsByPatientQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IMapper _mapper;

        public GetAppointmentsByPatientIdQueryHandler(IAppointmentRepo appointmentRepo, IMapper mapper)
        {
            _appointmentRepo = appointmentRepo;
            _mapper = mapper;
        }
        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByPatientQuery request, CancellationToken cancellationToken)
        {
            var appiontment = await _appointmentRepo.GetAppointmentByPatientIdAsync(request.PatientId);

            return _mapper.Map<List<AppointmentDto>>(appiontment);
        }
    }
}
