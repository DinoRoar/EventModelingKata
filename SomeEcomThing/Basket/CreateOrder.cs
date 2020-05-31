using System;
using Newtonsoft.Json;

namespace SomeEcomThing
{
    public class CreateOrder : Command
    {
        public Guid CustomerId { get; }
        public string OrderId { get; }

        public CreateOrder(Guid customerId, string orderId)
        {
            CustomerId = customerId;
            OrderId = orderId;
        }

        [JsonConstructor]
        private CreateOrder(Guid customerId, string orderId, string id) : base(id)
        {
            CustomerId = customerId;
            OrderId = orderId;
        }
    }
}