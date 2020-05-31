using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public interface IApply<in T> where T : Event
    {
        void Apply(T @event, long streamPosition);
    }
}