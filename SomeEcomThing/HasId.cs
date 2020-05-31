using System;

namespace SomeEcomThing
{

    public class HasId
    {
        public string GetId() => $"{this.GetType().Name}-{Guid.NewGuid()}";
    }
}
