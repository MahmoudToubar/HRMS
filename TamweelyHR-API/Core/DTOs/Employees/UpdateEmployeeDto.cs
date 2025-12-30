namespace Core.DTOs.Employees
{
    public class UpdateEmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public int JobId { get; set; }
    }
}
