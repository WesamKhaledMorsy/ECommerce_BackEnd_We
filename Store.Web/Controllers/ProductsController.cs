using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Service.Services.ProductServices;
using Store.Service.Services.ProductServices.DTOs;

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
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
        public async Task<ActionResult<IEnumerable<ProductDetailsDto>>> GetAllProducts()
          => Ok(await _productService.GetAllProductsAsync());
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> GetProductById(int? id)
    => Ok(await _productService.GetProductByIdAsync(id));
    }
}
