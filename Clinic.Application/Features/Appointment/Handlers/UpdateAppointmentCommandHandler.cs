using AutoMapper;
using Clinic.Application.Features.Appointment.Commands;
using Clinic.Domain.interfaces;
using Clinic.Domain.interfaces.repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Handlers
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAppointmentCommandHandler(IAppointmentRepo appointmentRepo, IUnitOfWork unitOfWork)
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
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
        }
    }
}
