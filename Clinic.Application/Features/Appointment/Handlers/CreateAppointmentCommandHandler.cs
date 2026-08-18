using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Appointment.Commands;
using Clinic.Application.Messages;
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
        private readonly IMessageProducer _messageProducer;

        public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork, IAppointmentRepo appointmentRepo, ICacheService cacheService , IMessageProducer messageProducer)
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _messageProducer = messageProducer;
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

            var message = new AppointmentCreatedMessage
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Title = appointment.Title,
                AppointmentDate = appointment.AppointmentDate
            };
            await _messageProducer.SendMessageAsync(message, "notifications_queue");
            return appointment.Id;
        }
    }
}