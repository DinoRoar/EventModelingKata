using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SomeEcomThing;
using SomeEcomThing.EventStore;
using Xunit;

namespace EventModelling.Spec
{
    public class CommandHandlerTestBase<T> where T : Command
    {
        protected readonly List<StreamEvent> Events = new List<StreamEvent>();

        private T _command;
        private IHandleCommand<T> _commandHandler;

        protected void Given(List<EventInStream> events)
        {
            foreach (var @event in events)
            {
                long streamPosition = Events.Count(e => e.StreamName == @event.Stream);
                long globalPosition = Events.Count;

                Events.Add(new StreamEvent(@event.Stream, streamPosition, globalPosition, DateTime.Now, @event.Event));
            }
        }

        protected void When(T command, IHandleCommand<T> commandHandler)
        {
            _command = command;
            _commandHandler = commandHandler;
        }

        protected void Then<TEvent>([NotNull] Action<TEvent> assertions) where TEvent : Event
        {
            _commandHandler.Load(Events);
            var actual = _commandHandler.Handle(_command);

            Assert.IsType<TEvent>(actual);
            assertions((TEvent)actual);
        }

        protected void ThenException<T>() where T:Exception
        {
            _commandHandler.Load(Events);
            Assert.Throws<T>(() => _commandHandler.Handle(_command));
        }
    }
}