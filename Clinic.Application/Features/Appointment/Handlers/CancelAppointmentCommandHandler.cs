using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Application.Common.Interfaces;
using Clinic.Application.Features.Appointment.Commands;
using Clinic.Application.Messages;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using MediatR;

namespace Clinic.Application.Features.Appointment.Handlers
{
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageProducer _messageProducer;

        public CancelAppointmentCommandHandler(
            IAppointmentRepo appointmentRepo,
            IUnitOfWork unitOfWork,
            IMessageProducer messageProducer)
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
            _messageProducer = messageProducer;
        }
        public async Task Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepo.GetAppointmentByIdAsync(request.AppointmentId);
            if (appointment == null) 
                    return;

            appointment.Cancel();

            await _appointmentRepo.UpdateAppointmentAsync(appointment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var canceledMessage = new AppointmentCanceledMessage
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Title = appointment.Title
            };

            await _messageProducer.SendMessageAsync(canceledMessage,"notifications_queue");
        }
    }
}