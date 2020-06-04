using System;
using System.Collections.Generic;
using SomeEcomThing;
using SomeEcomThing.Basket;
using SomeEcomThing.Order;
using Xunit;

namespace EventModelling.Spec
{
    public class CustomerChecksOutBasketCommandSpec : CommandHandlerTestBase<CheckOutBasket>
    {
        private const int Price = 1200;
        private const int ProductId = 34;
        private const string ProductTitle = "Shiny Thing";

        private readonly Guid _customerId = Guid.NewGuid();
        private readonly List<EventInStream> _eventInStreams;
        private readonly CheckOutBasket _checkoutBasketCommand;
        private readonly Guid _basketId = Guid.NewGuid();

        public CustomerChecksOutBasketCommandSpec()
        {
            var stream = $"basket-{_basketId}";
            var orderItem = new OrderItem(ProductTitle, ProductId, 1, Price );
            _eventInStreams = new List<EventInStream>
            {
                new EventInStream(stream,  new ItemAddedToBasket(_basketId, orderItem.ProductId, orderItem.Price, 2, orderItem.Title)),
                new EventInStream(stream, new ItemRemovedFromBasket(_basketId, ProductId, 1))
            };
            _checkoutBasketCommand = new CheckOutBasket(_basketId, _customerId);
        }

        [Fact]
        public void HappyPath()
        {
            Given(_eventInStreams);
            When(_checkoutBasketCommand, new CheckoutBasketCommandPolicy());
            var assertions = new Action<BasketCheckedOut>((actual) =>
           {
               Assert.IsType<BasketCheckedOut>(actual);
              Assert.Equal(_basketId, actual.BasketId);
              Assert.Equal(_customerId, actual.CustomerId);
           });

            Then(assertions);
        }
    }
}
