using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderSpecifications;
using Store.Service.Services.BasketServices;
using Store.Service.Services.BasketServices.DTOs;
using Store.Service.Services.OrderServices.DTOs;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Data.Entities.Product;

namespace Store.Service.Services.PaymentServices
{
    public class PaymentService:IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;


        public PaymentService(IConfiguration configuration,IUnitOfWork unitOfWork, IBasketService basketService, IMapper mapper)
        { 
            _configuration = configuration;
            _UnitOfWork = unitOfWork;
            _basketService = basketService;
            _mapper = mapper;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(CustomerBasketDto basket)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            if (basket is null)
                throw new Exception("Basket is Empty");
            var delivertyMethod = await _UnitOfWork.Repository<DeliverMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (delivertyMethod is null)
                throw new Exception("Delivery Method Not Provided");
            decimal shippingPrice = delivertyMethod.Price;
            foreach (var item in basket.BasketItems)
            {
                var product = await _UnitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
                if(item.Price != product.Price)  /// to check the price in basket is the same of the product in database
                    item.Price = product.Price;                    
            }
                // call stripe here
                var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            // check if I went here before or not to add on the last or new payment
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount=(long)basket.BasketItems.Sum(x=>x.Quantity * (x.Price * 100))+ (long)(basket.ShippingPrice*100 ),
                    Currency = "usd",
                    PaymentMethodTypes=new List<string> { "card"}
                };

                paymentIntent= await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount=(long)basket.BasketItems.Sum(x => x.Quantity * (x.Price * 100))+ (long)(basket.ShippingPrice*100),                   
                };

                await service.UpdateAsync(basket.PaymentIntentId,options);
            }
            
            await _basketService.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<OrderDetailsDto> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await _UnitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);
            if (order is null)
                throw new Exception("Order Does not Exist");

            order.OrderPaymentStatus= OrderPaymentStatus.Failed;
            _UnitOfWork.Repository<Order,Guid>().UpdateAsync(order);    
            await _UnitOfWork.CompleteAsync();
            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);
            return mappedOrder;
        }

        public async Task<OrderDetailsDto> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await _UnitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);
            if (order is null)
                throw new Exception("Order Does not Exist");

            order.OrderPaymentStatus= OrderPaymentStatus.Received   ;
            _UnitOfWork.Repository<Order, Guid>().UpdateAsync(order);
            await _UnitOfWork.CompleteAsync();
            await _basketService.DeleteBasketAsync(order.BasketId);
            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);
            return mappedOrder;
        }
    }
}
