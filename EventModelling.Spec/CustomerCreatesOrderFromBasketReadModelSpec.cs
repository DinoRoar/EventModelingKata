using System;
using System.Collections.Generic;
using SomeEcomThing;
using SomeEcomThing.Basket;
using Xunit;

namespace EventModelling.Spec
{
    public class CustomerBasketReadModelSpec : ReadModelTestBase<CustomerBasketReadModel>
    {
        private readonly string _productTitle;
        private readonly string _streamName;
        private readonly Guid _customerId;
        private readonly Guid _basketId;

        public CustomerBasketReadModelSpec()
        {
            _productTitle = "Product Title";
            _basketId = Guid.NewGuid();
            _streamName = $"basket-{_basketId}";
            _customerId = Guid.NewGuid();
        }

        [Fact]
        public void voidOnBasketCreated_ReadModelHasBasketForCustomer()
        {
            Given(new List<EventInStream>(), new CustomerBasketReadModel());
            When(new EventInStream(_streamName, new BasketCreated(_customerId, _basketId)));
            Then(actual =>
            {
                Assert.True(actual.BasketsByCustomer.ContainsKey(_customerId));
            });
        }

        [Fact]
        public void OnItemAddedToBasket_ReadModelHasItem()
        {
            Given(new List<EventInStream>()
            {
                new EventInStream(_streamName, new BasketCreated(_customerId, _basketId))
            }, new CustomerBasketReadModel());

            When(new EventInStream(_streamName, new ItemAddedToBasket(_basketId, 5, 1354, 3, _productTitle)));
            Then(actual =>
            {
                var basket = actual.BasketsByCustomer[_customerId];
                Assert.Equal(3, basket.NoItemsInBasket);
                Assert.True(basket.BasketItems.ContainsKey(5));
                var item = basket.BasketItems[5];
                Assert.Equal(1354, item.Price);
                Assert.Equal(3, item.Quantity);
                Assert.Equal(_productTitle, item.ProductTitle);
            });
        }

        [Fact]
        public void OnAddItemToBasketAgain_ReadModelHasCorrectQuantity()
        {
            Given(new List<EventInStream>()
            {
                new EventInStream(_streamName, new BasketCreated(_customerId, _basketId)),
                new EventInStream(_streamName, new ItemAddedToBasket(_basketId, 5, 1354, 3, _productTitle))
            }, new CustomerBasketReadModel());
            When(new EventInStream(_streamName, new ItemAddedToBasket(_basketId, 5, 1354, 3, _productTitle)));

            Then(actual =>
            {
                var basket = actual.BasketsByCustomer[_customerId];
                Assert.Equal(6, basket.NoItemsInBasket);
                Assert.True(basket.BasketItems.ContainsKey(5));
                var item = basket.BasketItems[5];
                Assert.Equal(6, item.Quantity);
            });
        }

        [Fact]
        public void OnRemoveItemFromBasket_ReadModelHasCorrectQuantity()
        {
            Given(new List<EventInStream>()
            {
                new EventInStream(_streamName, new BasketCreated(_customerId, _basketId)),
                new EventInStream(_streamName, new ItemAddedToBasket(_basketId,5, 1354, 3, _productTitle))
            }, new CustomerBasketReadModel());
            When(new EventInStream(_streamName, new ItemRemovedFromBasket(_basketId, 5, 1)));

            Then(actual =>
            {
                var basket = actual.BasketsByCustomer[_customerId];
                Assert.Equal(2, basket.NoItemsInBasket);
                Assert.True(basket.BasketItems.ContainsKey(5));
                var item = basket.BasketItems[5];
                Assert.Equal(2, item.Quantity);
            });
        }

        [Fact]
        public void OnRemoveAllOfItemFromBasket_ReadModelRemovesItem()
        {
            Given(new List<EventInStream>()
            {
                new EventInStream(_streamName, new BasketCreated(_customerId, _basketId)),
                new EventInStream(_streamName, new ItemAddedToBasket(_basketId,5, 1354, 3, _productTitle))
            }, new CustomerBasketReadModel());
            When(new EventInStream(_streamName, new ItemRemovedFromBasket(_basketId, 5, 4)));

            Then(actual =>
            {
                var basket = actual.BasketsByCustomer[_customerId];
                Assert.Equal(0, basket.NoItemsInBasket);
                Assert.False(basket.BasketItems.ContainsKey(5));
            });
        }
    }
}
