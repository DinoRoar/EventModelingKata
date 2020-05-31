using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SomeEcomThing.EventStore;

namespace SomeEcomThing
{
    public class CheckoutBasketCommandPolicy :
        IHandleCommand<CheckOutBasket>,
        IApply<ItemAddedToBasket>,
        IApply<ItemRemovedFromBasket>,
        IApply<BasketCheckedOut>
    {
        private readonly Dictionary<int, OrderItem> _items = new Dictionary<int, OrderItem>();
        private bool _isCheckedOut;

        public void Load(IEnumerable<StreamEvent> events)
        {
            foreach (var @event in events)
            {
                Apply((dynamic)@event.Event, @event.StreamPosition);
            }
        }

        public void Apply(object unhandled, long _)
        {
            Trace.Write($"Unhandled event in apply of CheckoutBasketCommandPolicy: {unhandled.GetType().Name}");
        }

        public void Apply(ItemAddedToBasket @event, long _)
        {
            var itemAlreadyInBasket = _items.ContainsKey(@event.ProductId);
            if (itemAlreadyInBasket)
            {
                AddQuantityToItemInBasket(@event);
            }
            else
            {
                AddNewItemToBasket(@event);
            }
        }

        private void AddNewItemToBasket(ItemAddedToBasket @event)
        {
            var orderItem = new OrderItem(@event.ProductTitle, @event.ProductId, @event.Quantity, @event.Price);
            _items.Add(orderItem.ProductId, orderItem);
        }

        private void AddQuantityToItemInBasket(ItemAddedToBasket @event)
        {
            _items[@event.ProductId] = _items[@event.ProductId].AddItems(@event.Quantity);
        }

        public void Apply(ItemRemovedFromBasket @event, long _)
        {
            var itemInBasket = _items.ContainsKey(@event.ProductId);
            if (!itemInBasket)
            {
                return;
            }

            var itemWithReducedQuantity = _items[@event.ProductId].RemoveItems(@event.Quantity);
            var stillInBasket = itemWithReducedQuantity.Quantity > 0;
            if (stillInBasket)
            {
                _items[@event.ProductId] = itemWithReducedQuantity;
            }
            else
            {
                _items.Remove(@event.ProductId);
            }
        }

        public void Apply(BasketCheckedOut @event, long _)
        {
            _isCheckedOut = true;
        }

        public Event Handle(CheckOutBasket command)
        {
            return new BasketCheckedOut(command.BasketId, command.CustomerId);
        }

        public class BasketNotCheckedOutException : InvalidOperationException
        {
            public BasketNotCheckedOutException(Guid customerId)
                : base($"Attempted to create an order before basket was checked out for customer: {customerId}")
            {
                Data.Add("customerId", customerId);
            }
        }
    }
}