using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderSpecifications;
using Store.Service.Services.BasketServices;
using Store.Service.Services.OrderServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderDetailsDto> CreateOrderAsync(OrderDto input)
        {
            //Get Basket >> to create the order
            var basket =await _basketService.GetBasketAsync(input.BasketId);
            if (basket is null)
                throw new Exception("Basket Not Exist");

            #region Fill Order Item List with Items in the basket
            var orderItems = new List<OrderItemDto>();
            foreach (var basketItem in orderItems)
            {
                var productItem = await _unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductItemId);
                if(productItem is null)
                    throw new Exception($"Product with Id: {basketItem.ProductItemId} Not Exist");

                var itemOrdered = new ProductItem
                {
                    ProductId=productItem.Id,
                    ProductName=productItem.Name,
                    PictureUrl=productItem.PictureUrl,
                };
                var orderItem = new OrderItem
                {
                    Price= productItem.Price,
                    Quatity = basketItem.Quatity,
                    ItemOrdered = itemOrdered
                };

                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mappedOrderItem); 
            }
            #endregion

            #region Get DeliveryMethod
            var deliveryMethod = await _unitOfWork.Repository<DeliverMethod, int>().GetByIdAsync(input.DeliveryMethodId);
            if (deliveryMethod is null) 
                throw new Exception("Delivery Mthod Not Provided");
            #endregion

            #region Calculate SubTotal
            var subTotal = orderItems.Sum(item=>item.Quatity * item.Price);
            #endregion

            #region TO Do => Payment

            #endregion

            #region Create Order
            var mappedShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order
            {
                DeliverMethodId= deliveryMethod.Id,
                ShippingAddress= mappedShippingAddress,
                BuyerEmail= input.BuyerEmail,
                BasketId= input.BasketId,
                OrderItems=mappedOrderItems,
                SubTotal=subTotal,
            };

            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await _unitOfWork.CompleteAsync();

            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);
            return mappedOrder;
            #endregion
        }

        public async Task<IReadOnlyList<DeliverMethod>> GetAllDeliveryMethodAsync(Guid id)
            => await _unitOfWork.Repository<DeliverMethod, int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderDetailsDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsAllAsync(specs);
            //if (orders is { Count : <=0 } )
            // Or
            if (!orders.Any())
                throw new Exception("You Do not have any orders Yet!");
            var mappedOrders = _mapper.Map<List<OrderDetailsDto>>(orders);
            return mappedOrders;

        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(Guid id)
        {
            var specs = new OrderWithItemSpecification(id);
            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);

            if (order is null)
                throw new Exception($"There is no order with Id {id}!");
            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);
            return mappedOrder;
        }
    }
}
