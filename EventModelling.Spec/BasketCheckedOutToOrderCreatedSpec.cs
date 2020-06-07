using System;
using System.Collections.Generic;
using SomeEcomThing;
using SomeEcomThing.Basket;
using SomeEcomThing.EventStore;
using SomeEcomThing.Order;
using Xunit;

namespace EventModelling.Spec
{
    public class BasketCheckedOutToOrderCreatedSpec : WorkerTestBase
    {
        [Fact]
        public void OnBasketCheckedOut_CreateOrder()
        {
            Given(new List<EventInStream>(), new BasketCheckedOutToOrderCreatedWorker());
            var basketId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var basketCheckedOut = new BasketCheckedOut(basketId,
                customerId, new List<BasketItem>()
                {
                    new BasketItem(3, 6, "some product", 1423)
                });

            var streamName = $"basket-{basketId.ToString()}";
            When(new StreamEvent(streamName, 0, 0, DateTime.Now,  basketCheckedOut));
            Then("order-1", streamEvent =>
            {
                var e = streamEvent.Event as OrderCreated;
                Assert.IsType<OrderCreated>(e);
                Assert.Equal(1, e.Order.OrderId);
                Assert.Equal(customerId, e.Order.CustomerId);
                Assert.Equal(basketId, e.Order.BasketId);
            });
        }
    }
}
