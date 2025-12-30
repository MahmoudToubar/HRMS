
namespace Core.Specifications
{
    public class EmployeeSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        private string _search = string.Empty;

        public string Search
        {
            get => _search;
            set => _search = string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim().ToLower();
        }

        private List<int> _departmentIds = [];
        public List<int> DepartmentIds
        {
            get => _departmentIds;
            set => _departmentIds = value;
        }

        private List<int> _jobIds = [];
        public List<int> JobIds
        {
            get => _jobIds;
            set => _jobIds = value;
        }

        public DateTime? DateOfBirthFrom { get; set; }
        public DateTime? DateOfBirthTo { get; set; }

        public string? Sort { get; set; }
    }
}
