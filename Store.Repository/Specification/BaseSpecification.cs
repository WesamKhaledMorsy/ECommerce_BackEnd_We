using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T,bool>> criteria)
        {
            Criteria=criteria;
        }
        // Intialize Criteria in constructor
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderByAsc { get; private set; }

        public Expression<Func<T, object>> OrderByDesc {  get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginated { get; private set; }
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
            => Includes.Add(includeExpression);

        protected void AddOrderByAsc(Expression<Func<T, object>> orderByExpression)
            => OrderByAsc = orderByExpression;

        protected void AddOrderByDesc(Expression<Func<T, object>> orderByExpressionDescending)
            => OrderByDesc  = orderByExpressionDescending;

        protected void ApplyPagination (int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPaginated = true;
        }
    }
}
