using Newtonsoft.Json;
using SomeEcomThing.Basket;

namespace SomeEcomThing.Order
{
    public class OrderItem
    {
        public string Title { get; }

        public int ProductId { get; }

        public int Quantity { get; }

        public int Price { get; }

        [JsonConstructor]
        public OrderItem(string title, int productId, int quantity, int price)
        {
            Title = title;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public OrderItem(BasketItem item)
        {
            Title = item.ProductTitle;
            ProductId = item.ProductId;
            Quantity = item.Quantity;
            Price = item.Price;
        }

        public OrderItem RemoveItems(in int quantity)
        {
            return new OrderItem(Title, ProductId, Quantity - quantity, Price);
        }

        public OrderItem AddItems(in int quantity)
        {
            return new OrderItem(Title, ProductId, Quantity + quantity, Price);
        }
    }
}