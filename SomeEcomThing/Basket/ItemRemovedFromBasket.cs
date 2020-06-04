using System;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing.Basket
{
    public class ItemRemovedFromBasket : Event
    {
        public Guid BasketId { get; }
        public int ProductId { get; }
        public int Quantity { get; }

        public ItemRemovedFromBasket(in Guid basketId, in int productId, int quantity)
        {
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
        }

        [JsonConstructor]
        private ItemRemovedFromBasket(Guid basketId, int productId, int quantity, string id) : base(id)
        {
            BasketId = basketId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}