using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Appointment.Commands;
<<<<<<< HEAD
using Clinic.Application.Messages;
=======
>>>>>>> feature/redis-and-caching-safe
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
<<<<<<< HEAD
        private readonly IMessageProducer _messageProducer;

        public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork, IAppointmentRepo appointmentRepo, ICacheService cacheService, IMessageProducer messageProducer)
=======

        public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork, IAppointmentRepo appointmentRepo, ICacheService cacheService)
>>>>>>> feature/redis-and-caching-safe
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
<<<<<<< HEAD
            _messageProducer = messageProducer;
=======
>>>>>>> feature/redis-and-caching-safe
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

<<<<<<< HEAD
            var message = new AppointmentCreatedMessage
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Title = appointment.Title,
                AppointmentDate = appointment.AppointmentDate
            };
            await _messageProducer.SendMessageAsync(message, "notifications_queue");

=======
>>>>>>> feature/redis-and-caching-safe
            return appointment.Id;
        }
    }
}