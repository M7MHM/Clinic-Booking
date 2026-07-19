using Clinic.Application.Features.Appointment.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Appointment.Vaildators
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty()
                .WithMessage("Doctor Id is required.");

            RuleFor(x => x.PatientId)
                .NotEmpty()
                .WithMessage("Patient Id is required.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Notes)
                .MaximumLength(200);

            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Appointment date must be in the future.");
        }
    }
}
