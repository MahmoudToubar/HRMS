namespace Core.Entities
{
    public class Employee : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public int JobId { get; set; }
        public Job Job { get; set; } = null!;
    }
}
