using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing.Order
{
    public class OrderCreated : Event
    {
        public Order Order { get; }

        [JsonConstructor]
        private OrderCreated(Order order, string id) : base(id)
        {
            Order = order;
        }

        public OrderCreated(Order order)
        {
            Order = order;
        }
    }
}