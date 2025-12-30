using Core.DTOs.Employees;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using FluentValidation;

namespace Core.Validators.EmployeeValidator
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator(IGenericRepository<Employee> repo)
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100)
                .MustAsync(async (email, ct) =>
                    !await repo.AnyAsync(e => e.Email == email))
                .WithMessage("Email already exists");

            RuleFor(x => x.Mobile)
                .NotEmpty()
                .Length(11)
                .Matches(@"^01[0-2,5][0-9]{8}$")
                .MustAsync(async (mobile, ct) =>
                    !await repo.AnyAsync(e => e.Mobile == mobile))
                .WithMessage("Mobile already exists");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today);

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0);

            RuleFor(x => x.JobId)
                .GreaterThan(0);
        }
    }
}
