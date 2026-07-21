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
                .WithMessage("The Name Is required.");

            RuleFor(x => x.Age)
                .InclusiveBetween(25, 80)
                .WithMessage("Patient age is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(100)
                .EmailAddress()
                .WithMessage("A valid email is required.");
        }
    }
}
