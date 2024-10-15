﻿using AutoMapper;
using Store.Data.Entities.IdentityEntities;
using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderServices.DTOs
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<ShippingAddress, AddressDto>().ReverseMap();
            CreateMap<Order, OrderDetailsDto>()
                .ForMember(dest => dest.DeliveryMethodName, options => options.MapFrom(src => src.DeliverMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, options => options.MapFrom(src => src.DeliverMethod.Price));
            
            CreateMap<OrderItem, OrderItemDto>()
              .ForMember(dest => dest.ProductItemId, options => options.MapFrom(src => src.ItemOrdered.ProductId))
              .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.ItemOrdered.ProductName))
              .ForMember(dest=>dest.PictureUrl,options=>options.MapFrom(src=>src.ItemOrdered.PictureUrl))
              .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemPictureUrlResolver>()).ReverseMap();
        }
    }
}
