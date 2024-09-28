using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    public interface ISpecification<T>
    {
        // 1.Criteria
        /// Criteria  .Where(x=>x.Id == id) 
        /// what in Where is A Criteria (expression of Func<>)
        Expression<Func<T, bool>> Criteria { get; }

        //2.Includes
        /// Includes
        //.Includes(x=>x.Id == id)
        /// what in Includes is A Criteria (expression of Func<>) but we will make list of Includes
        List<Expression<Func<T,object>>> Includes { get; }

        // 3. Ordering
        Expression<Func<T,object>> OrderByAsc {  get; }
        Expression<Func<T, object>> OrderByDesc { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; }

    }
}
