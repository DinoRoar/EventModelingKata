using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public class OrderCreated : Event
    {
        public List<OrderItem> OrderItems { get; }
        public Guid CustomerId { get; }
        public string OrderId { get; }

        public OrderCreated(List<OrderItem> orderItems, Guid customerId, string orderId)
        {
            OrderItems = orderItems;
            CustomerId = customerId;
            OrderId = orderId;
        }

        [JsonConstructor]
        private OrderCreated(List<OrderItem> orderItems, Guid customerId, string orderId, string id) : base(id)
        {
            OrderItems = orderItems;
            CustomerId = customerId;
        }
    }
}