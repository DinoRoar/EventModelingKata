using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeEcomThing
{
    public class Basket
    {
        public Guid CustomerId { get; }
        public Guid BasketId { get; }
        public int NoItemsInBasket => BasketItems.Values.Sum(i => i.Quantity);
        public Dictionary<int, BasketItem> BasketItems = new Dictionary<int, BasketItem>();

        public Basket(Guid basketId, Guid customerId)
        {
            BasketId = basketId;
            CustomerId = customerId;
        }
    }
}