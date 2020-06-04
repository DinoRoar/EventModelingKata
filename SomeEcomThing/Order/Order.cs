﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeEcomThing.Order
{
    public class Order
    {
        public string OrderId { get; }
        public Guid CustomerId { get; }
        public Guid BasketId { get; }
        public List<OrderItem> OrderItems { get; }

        public Order(Guid customerId, Guid basketId, string orderId, IEnumerable<OrderItem> orderItems)
        {
            CustomerId = customerId;
            BasketId = basketId;
            OrderId = orderId;
            OrderItems = orderItems.ToList();
        }
    }
}