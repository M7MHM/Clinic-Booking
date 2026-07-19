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
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IAppointmentRepo _appointmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork , IAppointmentRepo appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
            _unitOfWork = unitOfWork;
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
            return appointment.Id;
        }
    }
}
