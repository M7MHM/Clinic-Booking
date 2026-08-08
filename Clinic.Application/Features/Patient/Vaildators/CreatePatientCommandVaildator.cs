using Clinic.Application.Features.Patient.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Patient.Vaildators
{
    public class CreatePatientCommandVaildator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandVaildator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The name is required.")
                .MaximumLength(100)
                .WithMessage("The name must not exceed 100 characters.");

            RuleFor(x => x.Age)
                .InclusiveBetween(0, 120)
                .WithMessage("Age must be between 0 and 120.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .MaximumLength(100)
                .EmailAddress()
                .WithMessage("A valid email is required.");
        }
    }
}
