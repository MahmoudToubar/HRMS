using Core.DTOs.Employees;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using FluentValidation;

namespace Core.Validators.EmployeeValidator
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator(IGenericRepository<Employee> repo)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100)
                .MustAsync(async (dto, email, ct) =>
                    !await repo.AnyAsync(e => e.Email == email && e.Id != dto.Id))
                .WithMessage("Email already exists");

            RuleFor(x => x.Mobile)
                .NotEmpty()
                .Length(11)
                .Matches(@"^01[0-2,5][0-9]{8}$")
                .MustAsync(async (dto, mobile, ct) =>
                    !await repo.AnyAsync(e => e.Mobile == mobile && e.Id != dto.Id))
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
