using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Appointment.Commands;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using MediatR;

namespace Clinic.Application.Features.Appointment.Handlers
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public UpdateAppointmentCommandHandler(IAppointmentRepo appointmentRepo, IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepo.GetAppointmentByIdAsync(request.Id);

            if (appointment == null)
                return;

            appointment.Update(
                request.Title,
                request.AppointmentDate,
                request.Notes);

            await _appointmentRepo.UpdateAppointmentAsync(appointment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"appointment:{request.Id}");
            await _cacheService.RemoveAsync($"appointments:doctor:{appointment.DoctorId}");
            await _cacheService.RemoveAsync($"appointments:patient:{appointment.PatientId}");
        }
    }
}