using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderServices.DTOs
{

    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public readonly IConfiguration _configuration;
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(source.ItemOrdered.PictureUrl) || !source.ItemOrdered.PictureUrl.Contains(source.ItemOrdered.PictureUrl))
                return $"{_configuration["BaseUrl"]}/{source.ItemOrdered.PictureUrl}";
            return null;
        }
    }
}
