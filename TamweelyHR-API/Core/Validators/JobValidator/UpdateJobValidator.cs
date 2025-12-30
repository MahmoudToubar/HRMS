using Core.DTOs.Jobs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using FluentValidation;

namespace Core.Validators.JobValidator
{
    public class UpdateJobValidator : AbstractValidator<UpdateJobDto>
    {
        public UpdateJobValidator(IGenericRepository<Job> repo)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .MustAsync(async (dto, name, ct) =>
                    !await repo.AnyAsync(d => d.Name.ToLower() == name.ToLower() && d.Id != dto.Id))
                .WithMessage("Job name already exists");
        }
    }
}
