using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.ProductSpecifications
{
    public class ProductWithSpecifications:BaseSpecification<Product>
    {
        /// <summary>
        /// Here in ProductWithSpecifications inherits from BaseSpecification class where the constructor need Expression Of Func
        /// (the part will be passing to Where statement)
        /// </summary>
        /// <param name="productSpecification"></param>
        public ProductWithSpecifications(ProductSpecification productSpecification)
            :base(product => (!productSpecification.BrandId.HasValue || product.BrandId == productSpecification.BrandId.Value) 
                            && (!productSpecification.TypeId.HasValue || product.TypeId == productSpecification.TypeId.Value)
            &&(string.IsNullOrEmpty(productSpecification.Search) || product.Name.Trim().ToLower().Contains(productSpecification.Search))
            )
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);

            AddOrderByAsc(x => x.Name);
            AddOrderByDesc(x => x.Name);

            ApplyPagination(productSpecification.PageSize*(productSpecification.PageIndex -1), productSpecification.PageSize);

            if(!string.IsNullOrEmpty(productSpecification.Sort))
            {
                switch (productSpecification.Sort)
                {
                    case "PriceAsc":
                        AddOrderByAsc(x=>x.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(x=>x.Price);
                        break;
                    case "NameDesc":
                        AddOrderByDesc(x => x.Name);
                        break;
                    default :
                        AddOrderByAsc(x=>x.Name);
                        break;
                   
                }
            }
        }

        public ProductWithSpecifications(int? id):base(product => product.Id == id) 
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);                       
        }
    }
}
