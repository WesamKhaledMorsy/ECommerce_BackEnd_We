using Store.Repository.Specification.ProductBrandSpecifications;
using Store.Repository.Specification.ProductSpecifications;
using Store.Service.Helper;
using Store.Service.Services.ProductBrandServices.DTOs;
using Store.Service.Services.ProductServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductBrandServices
{
    public interface IProductBrandService
    {
        Task<ProductBrandDetailsDto> GetProductBrandByIdAsync(int? brandId);      
        Task<PaginatedResultDto<ProductBrandDetailsDto>> GetAllProductBrandsAsync(ProductBrandSpecification specs);//To Apply pagination
    }
}
