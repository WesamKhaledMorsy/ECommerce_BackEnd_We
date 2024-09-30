using Store.Data.Entities;
using Store.Repository.Specification.ProductSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.ProductBrandSpecifications
{
    public class ProductBrandCountSpecification:BaseSpecification<ProductBrand>
    {
        public ProductBrandCountSpecification(ProductBrandSpecification productBrandSpecification)
  : base(productbrand =>(string.IsNullOrEmpty(productBrandSpecification.Search) || productbrand.Name.Trim().ToLower().Contains(productBrandSpecification.Search))
  )
        {

        }
    }
}
