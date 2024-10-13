using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities.OrderEntities
{
    public class Order :BaseEntity<Guid>
    {
        public string BuyerEmail { get; set; }
        // As if use it among various countries
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddress ShippingAddress { get; set; }
        public DeliverMethod DeliverMethod { get; set; }
        public int? DeliverMethodId { get; set; }
        public OrderStatus OrderStatus { get; set; }= OrderStatus.Placed;
        public OrderPaymentStatus OrderPaymentStatus { get; set; }=OrderPaymentStatus.Pending;
        public IReadOnlyList<OrderItem> OrderItems { get; set;} 
        public string? BasketId     { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GetTotal()
            => SubTotal+DeliverMethod.Price;

        public string? PaymentIntentId { get; set; }

    }
}
