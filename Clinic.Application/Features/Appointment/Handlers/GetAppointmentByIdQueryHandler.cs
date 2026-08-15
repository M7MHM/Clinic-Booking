using AutoMapper;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Appointment.Dtos;
using Clinic.Application.Features.Appointment.Queries;
using Clinic.Domain.interfaces.repos;
using MediatR;

namespace Clinic.Application.Features.Appointment.Handlers
{
    public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAppointmentByIdQueryHandler(IAppointmentRepo appointmentRepo, IMapper mapper, ICacheService cacheService)
        {
            _appointmentRepo = appointmentRepo;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"appointment:{request.Id}";

            var cachedAppointment = await _cacheService.GetAsync<AppointmentDto>(cacheKey);
            if (cachedAppointment != null)
                return cachedAppointment;

            var appointment = await _appointmentRepo.GetAppointmentByIdAsync(request.Id);
            if (appointment == null)
                return null;

            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);

            await _cacheService.SetAsync(cacheKey, appointmentDto, TimeSpan.FromMinutes(5));

            return appointmentDto;
        }
    }
}