using Core.DTOs.Department;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using FluentValidation;
using System.Xml.Linq;

namespace Core.Validators.DepartmentValidator
{
    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDto>
    {
        public UpdateDepartmentValidator(IGenericRepository<Department> repo)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .MustAsync(async (dto, name, ct) =>
                    !await repo.AnyAsync(d => d.Name.ToLower() == name.ToLower() && d.Id != dto.Id))
                .WithMessage("Department name already exists");
        }
    }
}
