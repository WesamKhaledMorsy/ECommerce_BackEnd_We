using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Repository.Specification.ProductBrandSpecifications;
using Store.Repository.Specification.ProductSpecifications;
using Store.Service.Services.ProductBrandServices;
using Store.Service.Services.ProductBrandServices.DTOs;
using Store.Service.Services.ProductServices;
using Store.Service.Services.ProductServices.DTOs;

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductBrandController : ControllerBase
    {
        private readonly IProductBrandService _productBrandService;
        public ProductBrandController(IProductBrandService productBrandService)
        {
            _productBrandService = productBrandService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductBrandDetailsDto>>> GetAllProductBrands([FromQuery] ProductBrandSpecification input)
               => Ok(await _productBrandService.GetAllProductBrandsAsync(input));
        [HttpGet]
        public async Task<ActionResult<ProductBrandDetailsDto>> GetProductBrandById(int? id)
             => Ok(await _productBrandService.GetProductBrandByIdAsync(id));

    }
}
