using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductServices.DTOs
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDetailsDto, string>
    {
        public readonly IConfiguration _configuration;
        public ProductPictureUrlResolver(IConfiguration configuration )
        {
            _configuration = configuration;
        }
        // To put url before resolver   
        public string Resolve(Product source, ProductDetailsDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrWhiteSpace(source.PictureUrl)) 
                return $"{_configuration["BaseUrl"]}/{source.PictureUrl}";
            return null;
        }
    }
}
