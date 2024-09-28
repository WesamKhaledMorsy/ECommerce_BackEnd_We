using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification
{
    /// <summary>
    /// TEntity And TKey as the Evaluator recieves the Entity and its Id 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class SpecificationEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        // will recieve the base query and the additional query
        // to Combine the Base Query and the addtional Query we should use >> IQuerable << Interface

        //IEnumerable Vs IQuerable  >> Collection Of Data but different in Execution
        /// IEnumerable >> Execution is defered Exec >> get the data then process it and filter it in my memory after get it from Database
        /// 1. but when use it >>> when use Dictionary , List 
        /// 2.Small Data
        /// 3. Good Performance but that will different when Use large Data
        /// 4. in Linq >> L to o Linq to object

        /// IQuerable >> Execution is defered Exec >> but the execution in DataSource 
        /// 1. in Filteration , Sorting , Pagination , ...etc
        /// 2. Lagre Data 
        /// 3. L to e Linq to Entity


        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specs)
        {
            var query = inputQuery; 
            if(specs.Criteria is not null) 
                query = query.Where(specs.Criteria); // specs.Criteria >> x=>x.TypeId ==3 

            if(specs.OrderByAsc is not null) // x=>x.Name
                query = query.OrderBy(specs.OrderByAsc);
            if(specs.OrderByDesc is not null)
                query= query.OrderByDescending(specs.OrderByDesc);

            if(specs.IsPaginated)
                query =query.Skip(specs.Skip).Take(specs.Take);

            // Aggregate take an input then make some operations then add the result to as an input to make other operations 
            query= specs.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

            // the query my be at thenend like this 
            // IQuerable<Product>.Includes(x.Type).Where(x=>x.BrandId ==3);
            return query;
        }


    }
}
