using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
    }
}