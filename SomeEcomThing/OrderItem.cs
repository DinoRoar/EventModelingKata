namespace SomeEcomThing
{
    public class OrderItem
    {
        public OrderItem(string title, int productId, int quantity, int price)
        {
            Title = title;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public string Title { get; }
        public int ProductId { get; }
        public int Quantity { get; }
        public int Price { get; }

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