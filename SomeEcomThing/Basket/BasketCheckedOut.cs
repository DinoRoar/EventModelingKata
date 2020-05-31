using System;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public class BasketCheckedOut : Event
    {
        public Guid BasketId { get; }
        public Guid CustomerId { get; }

        public BasketCheckedOut(Guid basketId, Guid customerId)
        {
            BasketId = basketId;
            CustomerId = customerId;
        }

        [JsonConstructor]
        private BasketCheckedOut(Guid basketId,Guid customerId, string id) : base(id)
        {
            BasketId = basketId;
            CustomerId = customerId;
        }
    }
}