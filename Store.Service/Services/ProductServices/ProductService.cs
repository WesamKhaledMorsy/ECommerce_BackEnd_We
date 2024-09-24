using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Service.Services.ProductServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
             _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsNoTrackingAsync();
            var mappedBrands = _mapper.Map<IEnumerable<BrandTypeDetailsDto>>(brands);
            return mappedBrands;    
        }

        public async Task<IEnumerable<ProductDetailsDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product, int>().GetAllAsync();
            var mappedProducts = _mapper.Map<IEnumerable<ProductDetailsDto>>(products);
            return mappedProducts;
        }

        public async Task<IEnumerable<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var products = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();
            var mappedProducts = _mapper.Map<IEnumerable<BrandTypeDetailsDto>>(products);
            return mappedProducts;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? productId)
        {
            if (productId is null)
                throw new Exception("Id is Null");
            var product = await _unitOfWork.Repository<Product,int>().GetByIdAsync(productId.Value);
            if(product is null)
                throw new Exception("Product Not Found");
            var mappedProduct = _mapper.Map<ProductDetailsDto>(product);
            return mappedProduct;
        }
    }
}
