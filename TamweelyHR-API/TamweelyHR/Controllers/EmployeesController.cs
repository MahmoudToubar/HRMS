using AutoMapper;
using Core.DTOs.Employees;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IExcelExportService _excelExportService;
        private readonly IMapper _mapper;

        public EmployeesController(IGenericRepository<Employee> employeeRepo, IExcelExportService excelExportService,
            IMapper mapper)
        {
            _employeeRepo = employeeRepo;
            _excelExportService = excelExportService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult> GetEmployees([FromQuery] EmployeeSpecParams specParams)
        {
            var spec = new EmployeeSpecification(specParams);

            return await CreatePagedResult<Employee, EmployeeDto>(
                _employeeRepo, spec, specParams.PageIndex, specParams.PageSize,
                employees => _mapper.Map<IReadOnlyList<EmployeeDto>>(employees)
            );
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _employeeRepo.GetByIdAsync(id);
            if (employee == null || !employee.IsActive)
                return NotFound();

            return Ok(_mapper.Map<EmployeeDto>(employee));
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            dto.Mobile = dto.Mobile.Trim();
            dto.Email = dto.Email.Trim();

            var employee = _mapper.Map<Employee>(dto);
            employee.IsActive = true;

            _employeeRepo.Add(employee);
            await _employeeRepo.SaveAllAsync();

            return Ok();
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            dto.Id = id;

            dto.Mobile = dto.Mobile.Trim();
            dto.Email = dto.Email.Trim();

            var employee = await _employeeRepo.GetByIdAsync(id);
            if (employee == null || !employee.IsActive)
                return NotFound();

            _mapper.Map(dto, employee);
            _employeeRepo.Update(employee);
            await _employeeRepo.SaveAllAsync();

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDeleteEmployee(int id)
        {
            var employee = await _employeeRepo.GetByIdAsync(id);
            if (employee == null || !employee.IsActive)
                return NotFound();

            _employeeRepo.Delete(employee);
            await _employeeRepo.SaveAllAsync();

            return NoContent();
        }


        [HttpGet("export")]
        public async Task<IActionResult> ExportEmployees(
            [FromQuery] EmployeeSpecParams specParams)
        {
            var spec = new EmployeeSpecification(specParams);
            var employees = await _employeeRepo.ListAsync(spec);

            var fileBytes =
                _excelExportService.ExportEmployeesToExcel(employees);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Employees.xlsx"
            );
        }
    }
}
