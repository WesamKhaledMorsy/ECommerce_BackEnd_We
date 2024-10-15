using Store.Service.Services.BasketServices.DTOs;
using Store.Service.Services.OrderServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(CustomerBasketDto input); // This is only the end Point
        Task<OrderDetailsDto> UpdateOrderPaymentSucceeded(string paymentIntentId); // This is an action
        Task<OrderDetailsDto> UpdateOrderPaymentFailed(string paymentIntentId); // This is an action

    }
}
