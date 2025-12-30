using Core.DTOs.Jobs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using FluentValidation;

namespace Core.Validators.JobValidator
{
    public class CreateJobValidator : AbstractValidator<CreateJobDto>
    {
        public CreateJobValidator(IGenericRepository<Job> repo)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .MustAsync(async (name, ct) =>
                !await repo.AnyAsync(d => d.Name.ToLower() == name.ToLower()))
                .WithMessage("Job name already exists");
        }
    }
}
