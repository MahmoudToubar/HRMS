using Core.Entities;
using Core.Helpers;
using Core.Interfaces.Repositories;
using Core.Interfaces.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected async Task<ActionResult> CreatePagedResult<T, TResult> (IGenericRepository<T> repo, 
            ISpecification<T> spec, int pageIndex, int pageSize,
            Func<IReadOnlyList<T>, IReadOnlyList<TResult>> map) where T : BaseEntity
        {
            var items = await repo.ListAsync(spec);
            var count = await repo.CountAsync(spec);

            var mappedItems = map(items);

            var pagination = new Pagination<TResult>(pageIndex, pageSize, count, mappedItems);

            return Ok(pagination);
        }
    }
}
