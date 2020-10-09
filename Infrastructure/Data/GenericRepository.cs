using Core.Interfaces;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Core.Specifications;

// 33-2 Generic Repository
namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        // 34-1 Inject StoreContext at constructor.
        public GenericRepository(StoreContext context)
        {
            _context = context;
        }
       public async Task<T> GetByIdAsync(int id)
       {
           // 34-2 return what is needed from DbContext.
           // Set sets the Entity to be Working
           return await _context.Set<T>().FindAsync(id);
       } 
       public async Task<IReadOnlyList<T>> ListAllAsync()
       {
           return await _context.Set<T>().ToListAsync();
       }

       // 38-2 implementation of specification parametered method
        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            // evaluate IQueryable and execute query on last method call for desired type.
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        // 38-1 Apply specification
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            // get the evaluated IQueryable for the setted T type model for the specific specification spec.
            // here is where the EF context mix w/ the personalized IQueryable(The Specification).
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        // 65-3 implement count capability
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        // 218-4 Implement Unit Of Work new Method Interface
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        // 218-5 Implement Unit Of Work new Method Interface
        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        // 218-6 Implement Unit Of Work new Method Interface
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}