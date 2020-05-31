using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public interface IReadModel : IProjectEvents
    {
        public void SubscribeTo(IEventStore eventStore);
    }
}