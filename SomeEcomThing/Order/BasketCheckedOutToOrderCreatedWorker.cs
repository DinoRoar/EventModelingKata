using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SomeEcomThing.Basket;
using SomeEcomThing.EventStore;

namespace SomeEcomThing.Order
{
    /// <summary>
    /// This is a bit of a hack - passing in a command handler like this is a *bad* thing.   
    /// </summary>
    /// <remarks>
    /// Normally I would tell this to some kind of router. It might be local, it might be remote, it might be load balanced, it might be a cloud function (or Paul would be sad)
    /// I often use actor systems ...then you are not also forced to use a network protocol (but will automatically if it goes remote).
    /// The point is that you do not necessarily know where your system boundaries are or your scaling points will be.
    /// Developing simple generic patterns that free logic from needing to know is simple and powerful.
    /// </remarks>
    public class BasketCheckedOutToOrderCreatedWorker : 
        ITurnEventsIntoCommand, 
        IApply<BasketCheckedOut>
    {
        public void Load(IEnumerable<StreamEvent> events)
        {
            foreach (var @event in events)
            {
                Apply((dynamic)@event.Event, @event.StreamPosition);
            }
        }

        public void SubscribeTo(IEventStore eventStore)
        {
            eventStore.SubscribeToStream("et-BasketCheckedOut", streamEvent =>
            {
                var e = (BasketCheckedOut) streamEvent.Event;
                var orderItems = e.Items.Select(i=>new OrderItem(i)).ToList();
                var ordersCreatedStream = eventStore.ReadStream("et-orderCreated");
                var createOrderPolicy = new CreateOrderHandler();
                createOrderPolicy.Load(ordersCreatedStream);
                var orderCreated =
                    createOrderPolicy.Handle(new CreateOrder(e.CustomerId, e.BasketId, orderItems)) as OrderCreated;
                var @event = new StreamEvent(
                    $"order-{orderCreated.Order.OrderId}",
                    StreamPositions.CreateNewStream, 
                    StreamPositions.Any, 
                    DateTime.Now, 
                    orderCreated);
                eventStore.Append(@event);

            });
        }

        private string BuildOrderId(StreamEvent _)
        {
            return Guid.NewGuid().ToString();
        }

        public void Apply(object unhandled, long _)
        {
            Trace.Write($"Unhandled event in apply of BasketCheckedOutToOrderCreatedWorker: {unhandled.GetType().Name}");
        }

        public void Apply(BasketCheckedOut @event, long streamPosition)
        {
            throw new NotImplementedException();
        }
    }
}