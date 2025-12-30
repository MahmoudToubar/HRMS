using Core.DTOs.Department;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using FluentValidation;

namespace Core.Validators.DepartmentValidator
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator(IGenericRepository<Department> repo)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .MustAsync(async (name, ct) =>
                !await repo.AnyAsync(d => d.Name.ToLower() == name.ToLower()))

                .WithMessage("Department name already exists");
        }
    }
}
