using System;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public class BasketCreated : Event
    {
        public Guid CustomerId { get; }
        public Guid BasketId { get; }

        public BasketCreated(Guid customerId, Guid basketId)
        {
            CustomerId = customerId;
            BasketId = basketId;
        }

        [JsonConstructor]
        private BasketCreated(Guid customerId,Guid basketId, string id) : base(id)
        {
            CustomerId = customerId;
            BasketId = basketId;
        }
    }
}