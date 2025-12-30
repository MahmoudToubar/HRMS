using Core.Entities;
using Core.Specifications;

public class LookupSpecification<T> : BaseSpecification<T> where T : BaseLookupEntity
{
    public LookupSpecification(LookupSpecParams specParams)
        : base(x => x.IsActive && (string.IsNullOrEmpty(specParams.Search)
             || x.Name.ToLower().Contains(specParams.Search)))
    {

        switch (specParams.Sort)
        {
            case "name":
                AddOrderBy(x => x.Name);
                break;

            case "nameDesc":
                AddOrderByDescending(x => x.Name);
                break;

            default:
                AddOrderBy(x => x.Name);
                break;
        }
        
        ApplyPaging((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        
    }
}
