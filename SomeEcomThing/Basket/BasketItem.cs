namespace SomeEcomThing
{
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