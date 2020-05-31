namespace SomeEcomThing
{
    public class Command : HasId
    {
        public Command()
        {
            Id = GetId();
        }


        protected Command(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}