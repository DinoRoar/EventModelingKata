using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SomeEcomThing.Order
{
    public class CreateOrder : Command
    {
        public Guid CustomerId { get; }
        public Guid BasketId { get; }
        public List<OrderItem> OrderItems { get; }
      

        public CreateOrder(Guid customerId, Guid basketId, List<OrderItem> orderItems)
        {
            CustomerId = customerId;
            BasketId = basketId;
            OrderItems = orderItems;
        }

        [JsonConstructor]
        private CreateOrder(Guid customerId, Guid basketId, List<OrderItem> orderItems, string orderId, string id) : base(id)
        {
            CustomerId = customerId;
            BasketId = basketId;
            OrderItems = orderItems;
        }
    }
}