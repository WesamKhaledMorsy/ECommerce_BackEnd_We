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
        Task<IEnumerable<ProductDetailsDto>> GetAllProductsAsync();
        Task<IEnumerable<BrandTypeDetailsDto>> GetAllBrandsAsync();
        Task<IEnumerable<BrandTypeDetailsDto>> GetAllTypesAsync();
    }
}
