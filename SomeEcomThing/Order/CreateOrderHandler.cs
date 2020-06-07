using System;
using System.Collections.Generic;
using System.Diagnostics;
using SomeEcomThing.Basket;
using SomeEcomThing.EventStore;

namespace SomeEcomThing.Order
{
    public class CreateOrderHandler : IHandleCommand<CreateOrder>, IApply<OrderCreated>
    {
        private int _lastOrderId ;

        public Event Handle(CreateOrder command)
        {
            var orderId = GetNextOrderId(_lastOrderId);
            var order = new Order(command.CustomerId, command.BasketId, orderId, command.OrderItems);
            return new OrderCreated(order);
        }

        private int GetNextOrderId(int lastOrderId)
        {
            return lastOrderId + 1;

        }

        public void Load(IEnumerable<StreamEvent> events)
        {
            foreach (var @event in events)
            {
                Apply((dynamic)@event.Event, @event.StreamPosition);
            }
        }

        public void Apply(OrderCreated @event, long streamPosition)
        {
            _lastOrderId = @event.Order.OrderId;
        }
        
        public void Apply(object unhandled, long _)
        {
            Trace.Write($"Unhandled event in apply of CheckoutBasketCommandPolicy: {unhandled.GetType().Name}");
        }
    }
}