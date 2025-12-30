using Core.Entities;
using Core.Interfaces.Specifications;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveAllAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
