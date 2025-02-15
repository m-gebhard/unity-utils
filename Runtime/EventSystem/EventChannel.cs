using System;
using System.Collections.Generic;

namespace UnityUtils.EventSystem
{
    /// <summary>
    /// Abstract class representing an event channel that can subscribe, unsubscribe, and publish events.
    /// </summary>
    /// <typeparam name="TChannel">The type of the event channel.</typeparam>
    public abstract class EventChannel<TChannel> where TChannel : EventChannel<TChannel>
    {
        /// <summary>
        /// Dictionary to store event handlers by event type and handler ID.
        /// </summary>
        private readonly Dictionary<Type, Dictionary<int, Action<IEvent<TChannel>>>> handlers = new();

        /// <summary>
        /// Set of handler IDs to be removed after publishing.
        /// </summary>
        private readonly HashSet<int> handlersToRemove = new();

        /// <summary>
        /// Indicates whether an event is currently being published.
        /// </summary>
        private bool isPublishing;

        /// <summary>
        /// The next handler ID to be assigned.
        /// </summary>
        private int nextHandlerId;

        /// <summary>
        /// Subscribes an event by adding a handler to the list of handlers for the event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler to add for the event.</param>
        /// <returns>The ID of the subscribed handler.</returns>
        public int Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent<TChannel>
        {
            Type type = typeof(TEvent);

            if (!handlers.ContainsKey(type))
            {
                handlers[type] = new Dictionary<int, Action<IEvent<TChannel>>>();
            }

            int handlerId = nextHandlerId++;
            handlers[type][handlerId] = e => handler((TEvent)e);

            return handlerId;
        }

        /// <summary>
        /// Unsubscribes an event by removing a handler from the list of handlers for the event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handlerId">The ID of the handler to remove for the event.</param>
        public void Unsubscribe<TEvent>(int handlerId) where TEvent : IEvent<TChannel>
        {
            if (isPublishing)
            {
                handlersToRemove.Add(handlerId);
            }
            else
            {
                Type type = typeof(TEvent);

                if (handlers.TryGetValue(type, out var eventHandlers))
                {
                    eventHandlers.Remove(handlerId);
                }
            }
        }

        /// <summary>
        /// Unsubscribes all events by clearing the handlers dictionary.
        /// </summary>
        public void UnsubscribeAll()
        {
            handlers.Clear();
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

            isPublishing = true;

            foreach (var action in actions.Values)
            {
                action?.Invoke(@event);
            }

            isPublishing = false;

            foreach (var handlerId in handlersToRemove)
            {
                actions.Remove(handlerId);
            }

            handlersToRemove.Clear();
        }
    }
}