using System;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public class BasketCheckedOut : Event
    {
        public Guid BasketId { get; }

        public BasketCheckedOut(Guid basketId)
        {
            BasketId = basketId;
        }

        [JsonConstructor]
        private BasketCheckedOut(Guid basketId, string id) : base(id)
        {
            BasketId = basketId;
        }
    }
}