using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public class EventInStream
    {
        public string Stream { get; }
        public Event Event { get; }

        public EventInStream(string stream, Event @event)
        {
            Stream = stream;
            Event = @event;
        }
    }
}