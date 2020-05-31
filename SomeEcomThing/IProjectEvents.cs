using System.Collections.Generic;
using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public interface IProjectEvents
    {
        public void Load(IEnumerable<StreamEvent> events);
    }
}