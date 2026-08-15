using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Appointment.Commands;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using MediatR;

namespace Clinic.Application.Features.Appointment.Handlers
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork, IAppointmentRepo appointmentRepo, ICacheService cacheService)
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = new Domain.Tables.Appointment(
                request.DoctorId,
                request.PatientId,
                request.Title,
                request.AppointmentDate,
                request.Notes
            );

            await _appointmentRepo.AddAppointmentAsync(appointment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"appointments:doctor:{request.DoctorId}");
            await _cacheService.RemoveAsync($"appointments:patient:{request.PatientId}");

            return appointment.Id;
        }
    }
}