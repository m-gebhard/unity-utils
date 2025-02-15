using System;
using System.Collections.Generic;

namespace UnityUtils.EventSystem
{
    /// <summary>
    /// Abstract class representing an event channel that can subscribe and publish events.
    /// </summary>
    /// <typeparam name="TChannel">The type of the event channel.</typeparam>
    public abstract class EventChannel<TChannel> where TChannel : EventChannel<TChannel>
    {
        /// <summary>
        /// Dictionary to store event handlers by event type.
        /// </summary>
        private readonly Dictionary<Type, List<Action<IEvent<TChannel>>>> handlers = new();

        /// <summary>
        /// Subscribes an event by adding a handler to the list of handlers for the event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler to add for the event.</param>
        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent<TChannel>
        {
            Type type = typeof(TEvent);

            if (!handlers.ContainsKey(type))
            {
                handlers[type] = new List<Action<IEvent<TChannel>>>();
            }

            handlers[type].Add(WrappedHandler);
            return;

            void WrappedHandler(IEvent<TChannel> e) => handler((TEvent)e);
        }

        /// <summary>
        /// Unsubscribes an event by removing a handler from the list of handlers for the event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler to remove for the event.</param>
        /// TODO
        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent<TChannel>
        {
            Type type = typeof(TEvent);

            if (handlers.TryGetValue(type, out var eventHandlers))
            {
                eventHandlers.RemoveAll(h => h.Equals((Action<IEvent<TChannel>>)(e => handler((TEvent)e))));
            }
        }

        /// <summary>
        /// Publishes an event by invoking all handlers for the event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event to publish.</param>
        public void Publish<TEvent>(IEvent<TChannel> @event) where TEvent : IEvent<TChannel>
        {
            Type type = typeof(TEvent);

            if (!handlers.TryGetValue(type, out var actions)) return;

            foreach (var action in actions)
            {
                action?.Invoke(@event);
            }
        }
    }
}