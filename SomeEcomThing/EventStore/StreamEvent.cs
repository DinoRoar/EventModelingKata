using System;
using Newtonsoft.Json;

namespace SomeEcomThing.EventStore
{
    public class StreamEvent : Event
    {
        public string StreamName { get; }
        public long StreamPosition { get; protected set; }
        public long GlobalPosition { get; protected set; }
        public DateTime CreatedDate { get; }
        public Event Event { get; }

        public Event GetOriginatingEvent => GetOriginatingEventInternal(Event);
        public long GetOriginatingStreamPosition => GetOriginatingStreamPositionInternal(this);

        private static Event GetOriginatingEventInternal(Event @event)
        {
            if (EventIsExternal(@event))
            {
                return @event;
            }

            return GetOriginatingEventInternal(((StreamEvent)@event).Event);
        }

        private static bool EventIsExternal(Event @event)
        {
            return @event.GetType() != typeof(StreamEvent) && @event.GetType() != typeof(ProjectedEvent);
        }

        private static long GetOriginatingStreamPositionInternal(StreamEvent @event)
        {
            if (@event is ProjectedEvent projectedEvent)
            {
                return GetOriginatingStreamPositionInternal((StreamEvent)projectedEvent.Event);
            }


            if (@event.Event is StreamEvent streamEvent)
            {
                return GetOriginatingStreamPositionInternal(streamEvent);
            }

            return @event.StreamPosition;
        }



        /// <summary>
        /// Creates a stream event - On create the global position is ignored.
        /// </summary>
        /// <param name="streamName"></param>
        /// <param name="streamPosition"></param>
        /// <param name="globalPosition"></param>
        /// <param name="createdDate"></param>
        /// <param name="event"></param>
        public StreamEvent(string streamName, in long streamPosition, in long globalPosition, in DateTime createdDate,
            Event @event)
        {
            StreamName = streamName;
            StreamPosition = streamPosition;
            GlobalPosition = globalPosition;
            CreatedDate = createdDate;
            Event = @event;
        }

        [JsonConstructor]
        private StreamEvent(string id, string streamName, in long streamPosition, in long globalPosition, in DateTime createdDate, Event @event) : base(id)
        {
            StreamName = streamName;
            StreamPosition = streamPosition;
            GlobalPosition = globalPosition;
            CreatedDate = createdDate;
            Event = @event;
        }

        public virtual StreamEvent SetStreamPositions(int streamPosition, in int globalPosition)
        {
            return new StreamEvent(Id, StreamName, streamPosition, globalPosition, CreatedDate, Event);
        }
    }
}