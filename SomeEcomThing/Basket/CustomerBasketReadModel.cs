using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public class CustomerBasketReadModel : IReadModel, IApply<ItemAddedToBasket>, IApply<ItemRemovedFromBasket>, IApply<BasketCreated>
    {
        public Dictionary<Guid, Basket> BasketsByCustomer { get; } = new Dictionary<Guid, Basket>();
        public Dictionary<Guid, Basket> BasketsByBasket { get; } = new Dictionary<Guid, Basket>();

        public void Load(IEnumerable<StreamEvent> events)
        {
            foreach (var @event in events)
            {
                Apply((dynamic)@event.Event, @event.StreamPosition);
            }
        }

        public void SubscribeTo(IEventStore eventStore)
        {
            eventStore.SubscribeToStream("ca-basket", @event => Apply((dynamic)@event.GetOriginatingEvent, @event.GetOriginatingStreamPosition) );
        }

        public void Apply(object unhandled, long _)
        {
            Trace.Write($"Unhandled event in apply of customer Basket ReadModel: {unhandled.GetType().Name}");
        }

        public void Apply(BasketCreated @event, long streamPosition)
        {
            var basket = new Basket(@event.BasketId, @event.CustomerId);
            BasketsByBasket.Add(basket.BasketId, basket);
            BasketsByCustomer.Add(@event.CustomerId, basket);
        }

        public void Apply(ItemAddedToBasket @event, long streamPosition)
        {
            var basket = BasketsByBasket[@event.BasketId];
            if (basket.BasketItems.ContainsKey(@event.ProductId))
            {
                var basketItem = basket.BasketItems[@event.ProductId];
                basket.BasketItems[@event.ProductId] = basketItem.AddQuantity(@event.Quantity);
            }
            else
            {
                basket.BasketItems.Add(@event.ProductId, new BasketItem(@event.ProductId, @event.Quantity, @event.ProductTitle, @event.Price));
            }
        }

        public void Apply(ItemRemovedFromBasket @event, long streamPosition)
        {
            var basket = BasketsByBasket[@event.BasketId];
            if (!basket.BasketItems.ContainsKey(@event.ProductId))
            {
                return;
            }

            var item = basket.BasketItems[@event.ProductId].AddQuantity(-@event.Quantity);
            if (item.Quantity <= 0)
            {
                basket.BasketItems.Remove(@event.ProductId);
            }
            else
            {
                basket.BasketItems[@event.ProductId] = item;
            }
        }

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

        public class BasketItem
        {
            public int ProductId { get; }
            public int Quantity { get; }
            public string ProductTitle { get; }
            public int Price { get; }

            public BasketItem(in int productId, in int quantity, string productTitle, in int price)
            {
                ProductId = productId;
                Quantity = quantity;
                ProductTitle = productTitle;
                Price = price;
            }

            public BasketItem AddQuantity(in int quantity)
            {
                return new BasketItem(ProductId, Quantity + quantity, ProductTitle, Price);
            }
        }
    }
}