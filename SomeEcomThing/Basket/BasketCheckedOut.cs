using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing.Basket
{
    public class BasketCheckedOut : Event
    {
        public Guid BasketId { get; }
        public Guid CustomerId { get; }
        public List<BasketItem> Items { get; }

        public BasketCheckedOut(Guid basketId, Guid customerId, List<BasketItem> items)
        {
            BasketId = basketId;
            CustomerId = customerId;
            
            Items = items;
        }

        [JsonConstructor]
        private BasketCheckedOut(Guid basketId,Guid customerId, List<BasketItem> items, string id) : base(id)
        {
            BasketId = basketId;
            CustomerId = customerId;
            
            Items = items;
        }
    }
}