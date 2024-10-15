using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Service.HandleResponses;
using Store.Service.Services.OrderServices;
using Store.Service.Services.OrderServices.DTOs;
using System.Collections.Generic;
using System.Security.Claims;

namespace Store.Web.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService=orderService;
        }
        [HttpPost]
        public async Task<ActionResult<OrderDetailsDto>> CreateOrderAsync(OrderDto input)
        {
            var order = await _orderService.CreateOrderAsync(input);
            if (order is null)
                return BadRequest(new Response(400,"Error While Creating Your Order!"));
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsDto>>> GetAllOrdersForUserAsync()
        {
            // need to be Autorized
            var email = User.FindFirstValue(ClaimTypes.Email);  
            var orders = await _orderService.GetAllOrdersForUserAsync(email);   
            return Ok(orders);

        }
        [HttpGet]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderByIdAsync(Guid id)
        {
            // need to be Autorized
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliverMethod>>> GetAllDeliveryMethodAsync(Guid id)
        {
            var deliveryMethods= await _orderService.GetAllDeliveryMethodAsync(id);
            return deliveryMethods.ToList();
        }

    }
}
