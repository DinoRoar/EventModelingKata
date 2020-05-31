using System;

namespace SomeEcomThing
{
    public class CheckOutBasket : Command
    {
        public Guid BasketId { get; }
        public Guid CustomerId { get; }

        public CheckOutBasket(Guid basketId, Guid customerId)
        {
            BasketId = basketId;
            CustomerId = customerId;
        }
    }
}