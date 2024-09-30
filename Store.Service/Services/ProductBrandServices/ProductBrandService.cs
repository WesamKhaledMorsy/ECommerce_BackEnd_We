using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
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
    public class ProductBrandService : IProductBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductBrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       
        public async Task<PaginatedResultDto<ProductBrandDetailsDto>> GetAllProductBrandsAsync(ProductBrandSpecification input)
        {
            var specs = new ProductBrandWithSpecifications(input);
            //var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsNoTrackingAsync();
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetWithSpecificationsAllAsync(specs);

            var countSpecs = new ProductBrandCountSpecification(input);
            var count = await _unitOfWork.Repository<ProductBrand, int>().GetCountWithSpecificationAsync(countSpecs);
       
            var mappedBrands = _mapper.Map<IReadOnlyList<ProductBrandDetailsDto>>(brands);
            //return mappedBrands;
            return new PaginatedResultDto<ProductBrandDetailsDto>(input.PageIndex, input.PageSize, count, mappedBrands);
        }

        public async Task<ProductBrandDetailsDto> GetProductBrandByIdAsync(int? brandId)
        {
            if (brandId is null)
                throw new Exception("Id is Null");

            var specs = new ProductBrandWithSpecifications(brandId);
            var productBrand = await _unitOfWork.Repository<ProductBrand, int>().GetWithSpecificationsByIdAsync(specs);
            if (productBrand is null)
                throw new Exception("Brand Not Found");
            var mappedProduct = _mapper.Map<ProductBrandDetailsDto>(productBrand);
            return mappedProduct;
        }
    }
}
