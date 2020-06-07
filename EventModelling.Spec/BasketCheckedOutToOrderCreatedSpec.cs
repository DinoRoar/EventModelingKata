using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Security;
using System.Text;
using SomeEcomThing;
using SomeEcomThing.Basket;
using SomeEcomThing.EventStore;
using SomeEcomThing.Order;
using Xunit;

namespace EventModelling.Spec
{
    public class BasketCheckedOutToOrderCreatedSpec : WorkerTestBase
    {
        [Fact]
        public void OnBasketCheckedOut_CreateOrder()
        {
            Given(new List<EventInStream>(), new BasketCheckedOutToOrderCreatedWorker());
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var basketCheckedOut = new BasketCheckedOut(basketId,
                customerId, new List<BasketItem>()
                {
                    new BasketItem(3, 6, "some product", 1423)
                });

            var streamName = $"basket-{basketId.ToString()}";
            When(new StreamEvent(streamName, 0, 0, DateTime.Now,  basketCheckedOut));
            Then("order-1", streamEvent =>
            {
                var e = streamEvent.Event as OrderCreated;
                Assert.IsType<OrderCreated>(e);
                Assert.Equal(1, e.Order.OrderId);
                Assert.Equal(customerId, e.Order.CustomerId);
                Assert.Equal(basketId, e.Order.BasketId);
            });
        }

      
    }

    public class WorkerTestBase
    {
        private readonly IEventStore _eventStore = new InMemoryEventStore();
        private ITurnEventsIntoCommand _worker;

        protected void Given(List<EventInStream> events, ITurnEventsIntoCommand worker)
        {
            _worker = worker;
            _worker.SubscribeTo(_eventStore);

            var eventInStreams = events.ToList();
            foreach (var @event in eventInStreams)
            {
                var streamEvent = ToStreamEvent(@event);
                _eventStore.Append(streamEvent);
            }
        }
        
        protected void When(StreamEvent @event)
        {
           _eventStore.Append(@event);
        }
        
        protected void Then(string streamName, Action<StreamEvent> asserts)
        {
            var stream = _eventStore.ReadStream(streamName);
            Assert.NotEmpty(stream) ;
            var lastEvent = stream.Last();
            asserts(lastEvent);
        }

        private StreamEvent ToStreamEvent(EventInStream @event)
        {
            var events = _eventStore.ReadStream(@event.Stream);
            long streamPosition = 0;
            long globalPosition = 0;

            if (events.Any())
            {
                 streamPosition = events.Count;
                 globalPosition = events.Count;
            }
           

            var streamEvent = new StreamEvent(@event.Stream, streamPosition, globalPosition, DateTime.Now, @event.Event);
            return streamEvent;
        }
    }
}
