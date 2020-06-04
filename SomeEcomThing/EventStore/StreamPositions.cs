namespace SomeEcomThing.EventStore
{
    public class StreamPositions
    {
        public long StreamPosition { get; }
        public long GlobalPosition { get; }

        public StreamPositions(long streamPosition, long globalPosition)
        {
            StreamPosition = streamPosition;
            GlobalPosition = globalPosition;
        }

        public const long Any = -1;
        public const long CreateNewStream = -2;
    }
}