using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities;
using Store.Repository.Specification.ProductSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.ProductBrandSpecifications
{
    public class ProductBrandWithSpecifications:BaseSpecification<ProductBrand>
    {
        public ProductBrandWithSpecifications(ProductBrandSpecification productBrandSpecification)
           : base(productBrand => (string.IsNullOrEmpty(productBrandSpecification.Search) || productBrand.Name.Trim().ToLower().Contains(productBrandSpecification.Search))
           )
        {
         
            AddOrderByAsc(x => x.Name);
            AddOrderByDesc(x => x.Name);

            ApplyPagination(productBrandSpecification.PageSize*(productBrandSpecification.PageIndex -1), productBrandSpecification.PageSize);

            if (!string.IsNullOrEmpty(productBrandSpecification.Sort))
            {
                switch (productBrandSpecification.Sort)
                {                    
                    case "NameDesc":
                        AddOrderByDesc(x => x.Name);
                        break;
                    default:
                        AddOrderByAsc(x => x.Name);
                        break;

                }
            }
        }

        public ProductBrandWithSpecifications(int? id) : base(product => product.Id == id)
        {
            
        }
    }
}
