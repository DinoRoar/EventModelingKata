using System.Collections.Generic;
using System.Linq;
using System.Text;
using SomeEcomThing;
using SomeEcomThing.EventStore;
using SomeEcomThing.Order;
using Xunit;

namespace EventModelling.Spec
{
    public class BasketCheckedOutToOrderCreatedSpec : WorkerTestBase<BasketCheckedOutToOrderCreatedWorker>
    {
        [Fact]
        public void OnBasketCheckedOut_CreateOrder()
        {
            Given(new List<EventInStream>(), new BasketCheckedOutToOrderCreatedWorker());
        }
    }

    public class WorkerTestBase<TWorker> 
        where TWorker : ITurnEventsIntoCommand
    {
        private readonly IEventStore _eventStore = new InMemoryEventStore();
        private ITurnEventsIntoCommand _worker;

        protected void Given(List<EventInStream> events, TWorker worker)
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

        private StreamEvent ToStreamEvent(EventInStream @event)
        {
            long streamPosition = Events.Count(e => e.StreamName == @event.Stream);
            long globalPosition = Events.Count;

            var streamEvent = new StreamEvent(@event.Stream, streamPosition, globalPosition, DateTime.Now, @event.Event);
            return streamEvent;
        }
    }
}
