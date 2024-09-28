using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
             _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsNoTrackingAsync();
            var mappedBrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);
            return mappedBrands;    
        }

        //public async Task<IReadOnlyList<ProductDetailsDto>> GetAllProductsAsync()
        //{
        //    var products = await _unitOfWork.Repository<Product, int>().GetAllAsync();
        //    var mappedProducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);
        //    return mappedProducts;
        //}

        /// same as the above but with specifications
        //  public async Task<IReadOnlyList<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input)
        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input)
        {
            var specs = new ProductWithSpecifications(input);
            var products = await _unitOfWork.Repository<Product, int>().GetWithSpecificationsAllAsync(specs);
            // To get Count of all Products
            var countSpecs = new ProductWithCountSpecification(input);
            var count = await _unitOfWork.Repository<Product,int>().GetCountWithSpecificationAsync(countSpecs);

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);
            //return mappedProducts;
            return new PaginatedResultDto<ProductDetailsDto>(input.PageIndex, input.PageSize, count, mappedProducts);
        }
        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var products = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();
            var mappedProducts = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(products);
            return mappedProducts;
        }

        //public async Task<ProductDetailsDto> GetProductByIdAsync(int? productId)
        //{
        //    if (productId is null)
        //        throw new Exception("Id is Null");
        //    var product = await _unitOfWork.Repository<Product,int>().GetByIdAsync(productId.Value);
        //    if(product is null)
        //        throw new Exception("Product Not Found");
        //    var mappedProduct = _mapper.Map<ProductDetailsDto>(product);
        //    return mappedProduct;
        //}

        /// Get ById  with specifications
        public async Task<ProductDetailsDto> GetProductByIdAsync(int? productId)
        {
            if (productId is null)
                throw new Exception("Id is Null");

            var specs = new ProductWithSpecifications(productId);
            var product = await _unitOfWork.Repository<Product, int>().GetWithSpecificationsByIdAsync(specs);
            if (product is null)
                throw new Exception("Product Not Found");
            var mappedProduct = _mapper.Map<ProductDetailsDto>(product);
            return mappedProduct;
        }
    }
}
