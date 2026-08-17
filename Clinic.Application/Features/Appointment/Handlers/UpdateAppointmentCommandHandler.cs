<<<<<<< HEAD
﻿using AutoMapper;
using Clinic.Application.Common.Interfaces;
=======
﻿using Clinic.Application.Common.Interfaces;
>>>>>>> feature/redis-and-caching-safe
using Clinic.Application.Features.Appointment.Commands;
using Clinic.Application.Messages;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using MediatR;

namespace Clinic.Application.Features.Appointment.Handlers
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IUnitOfWork _unitOfWork;
<<<<<<< HEAD
        private readonly IMessageProducer _messageProducer;

        public UpdateAppointmentCommandHandler(IAppointmentRepo appointmentRepo, IUnitOfWork unitOfWork , IMessageProducer messageProducer)
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
            _messageProducer = messageProducer;
=======
        private readonly ICacheService _cacheService;

        public UpdateAppointmentCommandHandler(IAppointmentRepo appointmentRepo, IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
>>>>>>> feature/redis-and-caching-safe
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

<<<<<<< HEAD
            var updateMessage = new AppointmentUpdateMessage
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Title = appointment.Title,
                NewDate = appointment.AppointmentDate
            };

            await _messageProducer.SendMessageAsync(updateMessage , "notifications_queue");
=======
            await _cacheService.RemoveAsync($"appointment:{request.Id}");
            await _cacheService.RemoveAsync($"appointments:doctor:{appointment.DoctorId}");
            await _cacheService.RemoveAsync($"appointments:patient:{appointment.PatientId}");
>>>>>>> feature/redis-and-caching-safe
        }
    }
}