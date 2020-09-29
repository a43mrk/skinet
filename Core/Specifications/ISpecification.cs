using System;
using System.Collections.Generic;
using System.Linq.Expressions;

// 36-1 Contract for Specification.
namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // Criteria what of we will get.
        // Expression takes a function, a function takes a type and what is returning.
        Expression<Func<T, bool>> Criteria { get; }

        // Includes property will hold a list of include statements.
        //
        List<Expression<Func<T, object>>> Includes { get; }

        // 59-1 Adding Sort functionality
        Expression<Func<T, object>> OrderBy { get; }
        // 59-2 Adding Sort functionality
        Expression<Func<T, object>> OrderByDescending { get; }

        // 63-1 pagination
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}