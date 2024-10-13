using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.OrderSpecifications
{
    public class OrderWithItemSpecification :BaseSpecification<Order>
    {
        // for list
        public OrderWithItemSpecification(string buyerEmail)
            :base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order=> order.DeliverMethod);
            AddInclude(order=>order.OrderItems);
            AddOrderByDesc(order=> order.OrderDate);
        }
        // for one order
        public OrderWithItemSpecification(Guid id)
          : base(order => order.Id == id)
        {
            AddInclude(order => order.DeliverMethod);
            AddInclude(order => order.OrderItems);
        }
    }
}
