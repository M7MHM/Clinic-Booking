using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Application.Features.Appointment.Queries;
using Clinic.Domain.interfaces.repos;
using MediatR;

namespace Clinic.Application.Features.Appointment.Handlers
{
    public class GetAppointmentsByDoctorIdQueryHandler : IRequestHandler<GetAppointmentsByDoctorIdQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAppointmentsByDoctorIdQueryHandler(IAppointmentRepo appointmentRepo, IMapper mapper, ICacheService cacheService)
        {
            _appointmentRepo = appointmentRepo;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<AppointmentDto>> Handle(GetAppointmentsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"appointments:doctor:{request.DoctorId}";

            var cachedAppointments = await _cacheService.GetAsync<List<AppointmentDto>>(cacheKey);
            if (cachedAppointments != null)
                return cachedAppointments;

            var appointments = await _appointmentRepo.GetAppointmentByDoctorIdAsync(request.DoctorId);
            var appointmentsDto = _mapper.Map<List<AppointmentDto>>(appointments);

            await _cacheService.SetAsync(cacheKey, appointmentsDto, TimeSpan.FromMinutes(5));

            return appointmentsDto;
        }
    }
}