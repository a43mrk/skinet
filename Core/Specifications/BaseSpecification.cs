using System;
using System.Collections.Generic;
using System.Linq.Expressions;

// 36-2 Generic Base Specification
namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(){}

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        
        public Expression<Func<T, bool>> Criteria { get; }
        // initialize w/ empty list that will accumulate all related models to be included.
        public List<Expression<Func<T, object>>> Includes { get; } =
            new List<Expression<Func<T, object>>>();

        // method to add includes into internal list called Includes.
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // 59-3 implement new sorting methods
        public Expression<Func<T, object>> OrderBy { get; private set; }
        // 59-4 implement new sorting methods
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        // 59-5 make the expression sort setable
        protected void AddOrderBy(Expression<Func<T,object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        // 59-6 make the expression sort setable
        protected void AddOrderByDescending(Expression<Func<T,object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        // 63-2 implement pagination methods
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        // 63-3 method to set pagination settings
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}