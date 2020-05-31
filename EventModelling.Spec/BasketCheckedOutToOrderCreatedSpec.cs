using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SomeEcomThing;
using SomeEcomThing.EventStore;
using Xunit;

namespace EventModelling.Spec
{
    public class BasketCheckedOutToOrderCreatedSpec : WorkerTestBase<BasketCheckedOutToOrderCreatedWorker>
    {
        [Fact]
        public void OnBasketCheckedOut_CreateOrder()
        {

        }
    }

    public class BasketCheckedOutToOrderCreatedWorker : 
        IReadModel, 
        IApply<BasketCheckedOut>, 
        IApply<BasketCreated>, 
        IApply<ItemAddedToBasket>, 
        IApply<ItemRemovedFromBasket>
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

            });
        }

        public void Apply(object unhandled, long _)
        {
            Trace.Write($"Unhandled event in apply of BasketCheckedOutToOrderCreatedWorker: {unhandled.GetType().Name}");
        }

        public void Apply(BasketCheckedOut @event, long streamPosition)
        {
            throw new NotImplementedException();
        }

        public void Apply(BasketCreated @event, long streamPosition)
        {
            throw new NotImplementedException();
        }

        public void Apply(ItemAddedToBasket @event, long streamPosition)
        {
            throw new NotImplementedException();
        }

        public void Apply(ItemRemovedFromBasket @event, long streamPosition)
        {
            throw new NotImplementedException();
        }
    }

    public class WorkerTestBase<T>
    {
    }
}
