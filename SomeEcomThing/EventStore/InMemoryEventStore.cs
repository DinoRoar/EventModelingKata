﻿using System;
using System.Collections.Generic;

namespace SomeEcomThing.EventStore
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<string, List<StreamEvent>> _streams = new Dictionary<string, List<StreamEvent>>();
        private readonly List<StreamEvent> _allEvents = new List<StreamEvent>();
        private readonly List<Projection> _projections = new List<Projection>();
        private readonly Dictionary<string, List<Action<StreamEvent>>> _subscriptions = new Dictionary<string, List<Action<StreamEvent>>>();

        public InMemoryEventStore()
        {
            BuildDefaultProjections();
        }

        private void BuildDefaultProjections()
        {
            _projections.Add(new CategoryProjection());
            _projections.Add(new EventTypeProjection());
        }

        public void Append(StreamEvent streamEvent)
        {
            StreamEvent @event;
            var nextGlobalEventVersion = _allEvents.Count;
            if (!_streams.ContainsKey(streamEvent.StreamName))
            {
                CheckStreamPosition(streamEvent, 0);
                @event = streamEvent.SetStreamPositions(0, nextGlobalEventVersion);
                _streams.Add(@event.StreamName, new List<StreamEvent>() { });
            }
            else
            {
                var stream = _streams[streamEvent.StreamName];
                var nextPosition = stream.Count;

                if (streamEvent.StreamPosition != StreamPositions.Any)
                {
                    CheckStreamPosition(streamEvent, nextPosition);
                }

                @event = streamEvent.SetStreamPositions(nextPosition, nextGlobalEventVersion);
                stream.Add(@event);
            }

            _allEvents.Add(@event);

            ExecuteSubscriptions(@event);
            RunProjections(@event);
        }

        private void ExecuteSubscriptions(StreamEvent @event)
        {
            if (_subscriptions.ContainsKey(@event.StreamName))
            {
                _subscriptions[@event.StreamName].ForEach(s => s(@event));
            }
        }

        private void RunProjections(StreamEvent streamEvent)
        {
            foreach (var projection in _projections)
            {
                if (projection.Predicate(streamEvent))
                {
                    Append(new ProjectedEvent(projection.StreamName(streamEvent), streamEvent));
                }
            }
        }

        public void AddNewProjection(Projection projection)
        {
            _projections.Add(projection);
        }

        public void SubscribeToStream(string streamName, Action<StreamEvent> onEvent)
        {
            if (!_subscriptions.ContainsKey(streamName))
            {
                _subscriptions.Add(streamName, new List<Action<StreamEvent>>());
            }

            _subscriptions[streamName].Add(onEvent);
        }

        public List<StreamEvent> ReadStream(string streamName)
        {
            return _streams[streamName];
        }

        private static void CheckStreamPosition(StreamEvent streamEvent, long currentPosition)
        {
            if (streamEvent.StreamPosition != StreamPositions.Any && streamEvent.StreamPosition != currentPosition)
            {
                throw new IEventStore.InvalidStreamPosition(
                    streamEvent.StreamName,
                    streamEvent.StreamPosition,
                    currentPosition);
            }
        }
    }
}