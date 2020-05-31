using System;
using System.Collections.Generic;
using SomeEcomThing;
using Xunit;

namespace EventModelling.Spec
{
    public class CustomerCreatesOrderFromBasketCommandSpec : CommandHandlerTestBase<CreateOrder>
    {
        private const int Price = 1200;
        private const int ProductId = 34;
        private const string ProductTitle = "Shiny Thing";
        private const string OrderId = "Order1";

        private readonly string _stream;
        private readonly Guid _customerId = Guid.NewGuid();
        private readonly OrderItem _orderItem;
        private readonly List<EventInStream> _eventInStreams;
        private readonly CreateOrder _createOrderCommand;
        private readonly Guid _basketId = Guid.NewGuid();

        public CustomerCreatesOrderFromBasketCommandSpec()
        {
            _stream = $"basket-{_basketId}";
            _orderItem = new OrderItem(ProductTitle, ProductId, 1, Price );
            _eventInStreams = new List<EventInStream>
            {
                new EventInStream(_stream,  new ItemAddedToBasket(_basketId, _orderItem.ProductId, _orderItem.Price, 2, _orderItem.Title)),
                new EventInStream(_stream, new ItemRemovedFromBasket(_basketId, ProductId, 1)),
                new EventInStream(_stream, new BasketCheckedOut(_basketId))
            };
            _createOrderCommand = new CreateOrder(_customerId, OrderId);
        }

        [Fact]
        public void HappyPath()
        {
            Given(_eventInStreams);
            When(_createOrderCommand, new OrderCreator());

            var expectedOrderItems = new List<OrderItem>
            {
                _orderItem
            };
            var expectedEvent = new OrderCreated(expectedOrderItems, _customerId, OrderId);

            var assertions = new Action<OrderCreated>((actual) =>
           {
               Assert.IsType<OrderCreated>(actual);
               Assert.Equal(expectedEvent.OrderId, actual.OrderId);
               Assert.Equal(expectedEvent.CustomerId, actual.CustomerId);
               Assert.Collection(expectedEvent.OrderItems, actualOrderItem =>
               {
                   var expectedItem = expectedOrderItems[0];
                   Assert.Equal(expectedItem.ProductId, actualOrderItem.ProductId);
                   Assert.Equal(expectedItem.Price, actualOrderItem.Price);
                   Assert.Equal(expectedItem.Quantity, actualOrderItem.Quantity);
                   Assert.Equal(expectedItem.Title, actualOrderItem.Title);
               });
           });

            Then(assertions);
        }

        [Fact]
        public void NotCheckedOutBeforeOrderSpec()
        {
            _eventInStreams.RemoveAt(_eventInStreams.Count - 1);

            Given(_eventInStreams);

            When(_createOrderCommand, new OrderCreator());

            ThenException<OrderCreator.BasketNotCheckedOutException>();
        }
    }
}
