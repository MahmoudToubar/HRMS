using AutoMapper;
using Core.DTOs.Jobs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class JobsController : BaseApiController
    {
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly IMapper _mapper;

        public JobsController(IGenericRepository<Job> jobRepo, IMapper mapper)
        {
            _jobRepo = jobRepo;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetJobs([FromQuery] LookupSpecParams specParams)
        {
            var spec = new LookupSpecification<Job>(specParams);

            return await CreatePagedResult<Job, JobDto>(_jobRepo, spec, specParams.PageIndex, 
                specParams.PageSize, jobs => _mapper.Map<IReadOnlyList<JobDto>>(jobs));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<JobDto>> GetJob(int id)
        {
            var job = await _jobRepo.GetByIdAsync(id);
            if (job == null || !job.IsActive)
                return NotFound();

            return Ok(_mapper.Map<JobDto>(job));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateJob([FromBody] CreateJobDto dto)
        {
            dto.Name = dto.Name.Trim();

            var job = _mapper.Map<Job>(dto);
            job.IsActive = true;

            _jobRepo.Add(job);
            await _jobRepo.SaveAllAsync();

            return CreatedAtAction( nameof(GetJob), new { id = job.Id },
                _mapper.Map<JobDto>(job)
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobDto dto)
        {
            dto.Id = id;

            dto.Name = dto.Name.Trim();

            var job = await _jobRepo.GetByIdAsync(id);
            if (job == null || !job.IsActive)
                return NotFound();

            _mapper.Map(dto, job);
            _jobRepo.Update(job);
            await _jobRepo.SaveAllAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDeleteJob(int id)
        {
            var job = await _jobRepo.GetByIdAsync(id);
            if (job == null || !job.IsActive)
                return NotFound();

            _jobRepo.Delete(job);
            await _jobRepo.SaveAllAsync();

            return NoContent();
        }
    }
}
