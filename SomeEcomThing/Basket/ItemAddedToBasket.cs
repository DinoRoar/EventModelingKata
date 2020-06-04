using System;
using Newtonsoft.Json;
using SomeEcomThing.EventStore;

namespace SomeEcomThing.Basket
{
    public class ItemAddedToBasket : Event
    {
        public int ProductId { get; }
        public int Price { get; }
        public int Quantity { get; }
        public string ProductTitle { get; }

        public Guid BasketId { get; }

        [JsonConstructor]
        private ItemAddedToBasket(Guid basketId, int productId, int price, int quantity, string productTitle, string id) : base(id)
        {
            BasketId = basketId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
            ProductTitle = productTitle;
        }

        public ItemAddedToBasket(in Guid basketId, in int productId, in int price, in int quantity, string productTitle)
        {
            BasketId = basketId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
            ProductTitle = productTitle;
        }
    }
}