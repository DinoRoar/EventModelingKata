using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using SomeEcomThing;
using SomeEcomThing.EventStore;

namespace EventModelling.Spec
{
    /// <summary>
    /// Note this uses subscriptions to an in memory event store to update the readmodel.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadModelTestBase<T> where T : IReadModel
    {
        private T _readModel;
        private readonly IEventStore _eventStore;

        public ReadModelTestBase()
        {
            _eventStore = new InMemoryEventStore();
        }

        protected void Given(IEnumerable<EventInStream> events, T readModel)
        {
            _readModel = readModel;
            _readModel.SubscribeTo(_eventStore);
            
            var eventInStreams = events.ToList();
            foreach (var @event in eventInStreams)
            {
                var streamEvent = ToStreamEvent(@event);
                _eventStore.Append(streamEvent);
            }
        }

        private StreamEvent ToStreamEvent(EventInStream @event)
        {
            var positions = _eventStore.GetPosition(@event.Stream);
            var events = _eventStore.ReadStream(@event.Stream);
            long streamPosition = 0;
            long globalPosition = 0;
            if (events.Any())
            {
                streamPosition = events.Last().StreamPosition + 1;
                globalPosition = events.Last().GlobalPosition + 1;
            }
          

            var streamEvent = new StreamEvent(@event.Stream, streamPosition, globalPosition, DateTime.Now, @event.Event);
            return streamEvent;
        }

        protected void When(EventInStream @event)
        {
            var streamEvent = new StreamEvent(@event.Stream, StreamPositions.Any, StreamPositions.Any, DateTime.Now, @event.Event);
            _eventStore.Append(streamEvent);
        }

        protected void Then(Action<T> asserts)
        {
            asserts(_readModel);
        }
    }
}