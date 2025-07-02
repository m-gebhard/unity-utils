using System;
using System.Collections.Generic;

namespace UnityUtils.EventSystem
{
    /// <summary>
    /// Interface representing an event channel.
    /// </summary>
    public interface IEventChannel
    {
        public void UnsubscribeAll();
    }

    /// <summary>
    /// Abstract class representing an event channel that can subscribe, unsubscribe, and publish events.
    /// </summary>
    /// <typeparam name="TChannel">The type of the event channel.</typeparam>
    public abstract class EventChannel<TChannel> : IEventChannel where TChannel : EventChannel<TChannel>
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
        /// Indicates whether the handlers dictionary needs to be cleared after publishing.
        /// </summary>
        private bool needsClearAfterPublishing;

        /// <summary>
        /// Subscribes a handler to an event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler to be invoked when the event is published.</param>
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
        /// Unsubscribes a handler from an event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handlerId">The ID of the handler to be unsubscribed.</param>
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
        /// Unsubscribes all event handlers.
        /// </summary>
        public void UnsubscribeAll()
        {
            if (isPublishing)
            {
                needsClearAfterPublishing = true;
            }
            else
            {
                handlers.Clear();
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
            if (!handlers.TryGetValue(type, out Dictionary<int, Action<IEvent<TChannel>>> actions)) return;

            isPublishing = true;

            foreach (var action in actions.Values)
            {
                action?.Invoke(@event);
            }

            isPublishing = false;

            HandleHandlersToRemove(actions);
        }

        /// <summary>
        /// Handles the removal of handlers that were marked for removal during publishing.
        /// </summary>
        /// <param name="actions">The dictionary of actions to remove handlers from.</param>
        private void HandleHandlersToRemove(Dictionary<int, Action<IEvent<TChannel>>> actions)
        {
            foreach (var handlerId in handlersToRemove)
            {
                actions.Remove(handlerId);
            }

            handlersToRemove.Clear();

            if (needsClearAfterPublishing)
            {
                handlers.Clear();
                needsClearAfterPublishing = false;
            }
        }
    }
}