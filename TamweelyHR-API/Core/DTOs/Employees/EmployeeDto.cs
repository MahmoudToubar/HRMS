namespace Core.DTOs.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public int JobId { get; set; }
        public string JobName { get; set; } = null!;
    }
}
