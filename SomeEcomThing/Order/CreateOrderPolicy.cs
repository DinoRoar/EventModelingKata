using System.Collections.Generic;
using SomeEcomThing.Basket;
using SomeEcomThing.EventStore;

namespace SomeEcomThing.Order
{
    public class CreateOrderPolicy : IHandleCommand<CreateOrder>
    {
        public Event Handle(CreateOrder command)
        {
            return new OrderCreated(command.Order);
        }

        public void Load(IEnumerable<StreamEvent> events)
        {
            
        }
    }
}