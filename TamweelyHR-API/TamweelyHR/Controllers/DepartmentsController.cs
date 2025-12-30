using AutoMapper;
using Core.DTOs.Department;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseApiController
    {
        private readonly IGenericRepository<Department> _departmentRepo;
        private readonly IMapper _mapper;

        public DepartmentsController(
            IGenericRepository<Department> departmentRepo,
            IMapper mapper)
        {
            _departmentRepo = departmentRepo;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetDepartments([FromQuery] LookupSpecParams specParams)
        {
            var spec = new LookupSpecification<Department>(specParams);

            return await CreatePagedResult<Department, DepartmentDto>(_departmentRepo, spec, specParams.PageIndex,
                specParams.PageSize, departments => _mapper.Map<IReadOnlyList<DepartmentDto>>(departments));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            var department = await _departmentRepo.GetByIdAsync(id);
            if (department == null || !department.IsActive)
                return NotFound();

            return Ok(_mapper.Map<DepartmentDto>(department));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto)
        {
            dto.Name = dto.Name.Trim();

            var department = _mapper.Map<Department>(dto);
            department.IsActive = true;

            _departmentRepo.Add(department);
            await _departmentRepo.SaveAllAsync();

            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id },
                _mapper.Map<DepartmentDto>(department)
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto dto)
        {
            dto.Id = id;

            dto.Name = dto.Name.Trim();

            var department = await _departmentRepo.GetByIdAsync(id);
            if (department == null || !department.IsActive)
                return NotFound();

            _mapper.Map(dto, department);
            _departmentRepo.Update(department);
            await _departmentRepo.SaveAllAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDeleteDepartment(int id)
        {
            var department = await _departmentRepo.GetByIdAsync(id);
            if (department == null || !department.IsActive)
                return NotFound();

            _departmentRepo.Delete(department); 
            await _departmentRepo.SaveAllAsync();

            return NoContent();
        }
    }
}
