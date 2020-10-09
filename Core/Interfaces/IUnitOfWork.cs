using System;
using Core.Entities;
using System.Threading.Tasks;

// 217-1 Interface for Unit of Work
namespace Core.Interfaces
{
    // 217-2 extends IDisposable to the system look for dispose method, to dispose our context
    public interface IUnitOfWork : IDisposable
    {
        // holds repositories of entities
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        // return the number of changes
        Task<int> Complete();
    }
}