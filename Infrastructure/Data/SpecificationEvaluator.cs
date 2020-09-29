using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

// 37-1 Specification Evaluator, needed to apply desired IQueryable that fits needed functionality.
// restricted to Entity types.
// where specifications are consumed to return an IQueryable that do the right query
namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        // require IQueryable from DbContext that comes from repository and an specification.
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            // evaluate criteria
            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if(spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Evaluate includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
        
    }
}