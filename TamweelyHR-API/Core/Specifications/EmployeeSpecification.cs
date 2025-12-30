using Core.Entities;

namespace Core.Specifications
{
    public class EmployeeSpecification : BaseSpecification<Employee>
    {
        public EmployeeSpecification(EmployeeSpecParams specParams)
            : base(e => e.IsActive && (string.IsNullOrEmpty(specParams.Search) || e.FullName.ToLower().Contains(specParams.Search)
                    || e.Email.ToLower().Contains(specParams.Search)
                    || e.Mobile.Contains(specParams.Search)) && (specParams.DepartmentIds.Count == 0
                    || specParams.DepartmentIds.Contains(e.DepartmentId)) &&

                (specParams.JobIds.Count == 0 || specParams.JobIds.Contains(e.JobId)) && (!specParams.DateOfBirthFrom.HasValue
                    || e.DateOfBirth >= specParams.DateOfBirthFrom) &&

                (!specParams.DateOfBirthTo.HasValue
                    || e.DateOfBirth <= specParams.DateOfBirthTo)
            )
        {
            AddInclude(e => e.Department);
            AddInclude(e => e.Job);

            switch (specParams.Sort)
            {
                case "name":
                    AddOrderBy(e => e.FullName);
                    break;

                case "nameDesc":
                    AddOrderByDescending(e => e.FullName);
                    break;

                case "email":
                    AddOrderBy(e => e.Email);
                    break;

                case "emailDesc":
                    AddOrderByDescending(e => e.Email);
                    break;

                case "dateOfBirth":
                    AddOrderBy(e => e.DateOfBirth);
                    break;

                case "dateOfBirthDesc":
                    AddOrderByDescending(e => e.DateOfBirth);
                    break;

                default:
                    AddOrderBy(e => e.FullName);
                    break;
            }

            ApplyPaging(
                specParams.PageSize * (specParams.PageIndex - 1),
                specParams.PageSize
            );
        }
    }
}
