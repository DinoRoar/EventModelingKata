using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public class BasketCheckedOut : Event
    {
        public Guid BasketId { get; }
        public Guid CustomerId { get; }
        public Dictionary<int, BasketItem> Items { get; }

        public BasketCheckedOut(Guid basketId, Guid customerId, Dictionary<int, BasketItem> items)
        {
            BasketId = basketId;
            CustomerId = customerId;
            Items = items;
        }

        [JsonConstructor]
        private BasketCheckedOut(Guid basketId,Guid customerId, Dictionary<int, BasketItem> items, string id) : base(id)
        {
            BasketId = basketId;
            CustomerId = customerId;
            Items = items;
        }
    }
}