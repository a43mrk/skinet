
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

// 33-1 Generic Interface for Generic Repository
namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();

        // 37-2 make generic repository works with specification.
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        // 65-2 adding count capability
        Task<int> CountAsync(ISpecification<T> spec);

        // 218-1 Methods to Use Unit Of Work
        void Add(T entity);
        // 218-2 Methods to Use Unit Of Work
        void Update(T entity);
        // 218-3 Methods to Use Unit Of Work
        void Delete(T entity);
    }
}