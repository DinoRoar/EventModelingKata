using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SomeEcomThing.Basket
{
    public class CheckOutBasket : Command
    {
        public Guid BasketId { get; }
        public Guid CustomerId { get; }
        public List<BasketItem> Items { get; }

        public CheckOutBasket(Guid basketId, Guid customerId, List<BasketItem> items)
        {
            BasketId = basketId;
            CustomerId = customerId;
            Items = items;
        }

        [JsonConstructor]
        private CheckOutBasket(Guid basketId, Guid customerId, List<BasketItem> items, string id) : base(id)
        {
            BasketId = basketId;
            CustomerId = customerId;
            Items = items;
        }
    }
}