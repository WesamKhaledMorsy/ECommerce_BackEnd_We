using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.ProductSpecifications
{
    public class ProductWithCountSpecification:BaseSpecification<Product>
    {
        public ProductWithCountSpecification(ProductSpecification productSpecification)
          : base(product => (!productSpecification.BrandId.HasValue || product.BrandId == productSpecification.BrandId.Value)
                          && (!productSpecification.TypeId.HasValue || product.TypeId == productSpecification.TypeId.Value)
                         &&(string.IsNullOrEmpty(productSpecification.Search) || product.Name.Trim().ToLower().Contains(productSpecification.Search))
          )
        {

        }
    }
}
