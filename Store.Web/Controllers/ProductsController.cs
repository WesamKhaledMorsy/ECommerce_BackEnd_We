using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Repository.Specification.ProductSpecifications;
using Store.Service.Services.ProductServices;
using Store.Service.Services.ProductServices.DTOs;
using Store.Web.Helper;

namespace Store.Web.Controllers
{

    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        //public async Task< IEnumerable<BrandTypeDetailsDto>> GetAllBrands()
        //    => await _productService.GetAllBrandsAsync();
        public async Task<ActionResult< IEnumerable<BrandTypeDetailsDto>>> GetAllBrands()
            => Ok(await _productService.GetAllBrandsAsync());

        [HttpGet]   
        public async Task<ActionResult<IEnumerable<BrandTypeDetailsDto>>> GetAllTypes()
           => Ok(await _productService.GetAllTypesAsync());
        [HttpGet]
        [Cache(30)]
        public async Task<ActionResult<IEnumerable<ProductDetailsDto>>> GetAllProducts([FromQuery]ProductSpecification input)
          => Ok(await _productService.GetAllProductsAsync(input));
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> GetProductById(int? id)
             => Ok(await _productService.GetProductByIdAsync(id));
    }
}
