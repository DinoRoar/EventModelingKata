using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using SomeEcomThing;
using SomeEcomThing.EventStore;

namespace EventModelling.Spec
{
    public class ReadModelTestBase<T> where T : IReadModel
    {
        protected List<StreamEvent> Events = new List<StreamEvent>();
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
            long streamPosition = Events.Count(e => e.StreamName == @event.Stream);
            long globalPosition = Events.Count;

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