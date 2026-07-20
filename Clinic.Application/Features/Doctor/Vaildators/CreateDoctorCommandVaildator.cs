using Clinic.Application.Features.Doctor.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.Doctor.Vaildators
{
    public class CreateDoctorCommandVaildator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandVaildator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The Name Is required.");

            RuleFor(x => x.Age)
                .InclusiveBetween(25, 80) 
                .WithMessage("Doctor age must be between 25 and 80.");

            RuleFor(x => x.Specialization)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(100)
                .EmailAddress()
                .WithMessage("A valid email is required.");
        }
    }
}
