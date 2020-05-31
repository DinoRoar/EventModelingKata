using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public interface IHandleCommand<in T> : IProjectEvents where T : Command
    {
        public Event Handle(T command);
    }
}