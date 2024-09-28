using Store.Repository.Specification.ProductSpecifications;
using Store.Service.Helper;
using Store.Service.Services.ProductServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductServices
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetProductByIdAsync(int? productId);
        //Task<IReadOnlyList<ProductDetailsDto>> GetAllProductsAsync();
        //Task<IReadOnlyList<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification specs);        
        Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification specs);//To Apply pagination

        Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync();
        Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync();
    }
}
