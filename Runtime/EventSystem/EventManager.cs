using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils.GameObjects;

namespace UnityUtils.EventSystem
{
    /// <summary>
    /// Manages event channels and provides methods to subscribe and publish events.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class EventManager : PersistentSingleton<EventManager>
    {
        /// <summary>
        /// Dictionary to store event channels by their type.
        /// </summary>
        private static readonly Dictionary<Type, IEventChannel> EventChannels = new();

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;
            RegisterEventChannels();
        }

        /// <summary>
        /// Registers all event channels by finding and instantiating classes that extend EventChannel.
        /// </summary>
        private static void RegisterEventChannels()
        {
            // Get all classes that extend EventChannel.
            var types = PredefinedAssemblyUtil.GetTypes(typeof(EventChannel<>));

            // Create an instance of each channel.
            foreach (Type channelType in types)
            {
                IEventChannel createdEventChannel = (IEventChannel)Activator.CreateInstance(channelType);
                EventChannels.Add(channelType, createdEventChannel);
            }
        }

        /// <summary>
        /// Gets the event channel of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the event channel.</typeparam>
        /// <returns>The event channel of the specified type.</returns>
        private static T GetEventChannel<T>() where T : EventChannel<T>
        {
            return (T)EventChannels[typeof(T)];
        }

        /// <summary>
        /// Subscribes to an event by adding a handler to the event channel.
        /// </summary>
        /// <typeparam name="TChannel">The type of the event channel.</typeparam>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The handler to add for the event.</param>
        /// <returns>The ID of the subscribed handler.</returns>
        public static int Subscribe<TChannel, TEvent>(Action<TEvent> handler)
            where TChannel : EventChannel<TChannel>
            where TEvent : IEvent<TChannel>
        {
            return GetEventChannel<TChannel>().Subscribe<TEvent>(e => handler((TEvent)e));
        }

        /// <summary>
        /// Unsubscribes from an event by removing a handler from the event channel.
        /// </summary>
        /// <typeparam name="TChannel">The type of the event channel.</typeparam>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handlerId">The ID of the handler to remove for the event.</param>
        public static void Unsubscribe<TChannel, TEvent>(int handlerId)
            where TChannel : EventChannel<TChannel>
            where TEvent : IEvent<TChannel>
        {
            GetEventChannel<TChannel>().Unsubscribe<TEvent>(handlerId);
        }

        /// <summary>
        /// Unsubscribes all events by clearing the handlers dictionary in the specified event channel.
        /// </summary>
        protected static void UnsubscribeAll<TChannel>() where TChannel : EventChannel<TChannel>
        {
            GetEventChannel<TChannel>().UnsubscribeAll();
        }

        /// <summary>
        /// Publishes an event by invoking all handlers for the event type.
        /// </summary>
        /// <typeparam name="TChannel">The type of the event channel.</typeparam>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event to publish.</param>
        public static void Publish<TChannel, TEvent>(TEvent @event)
            where TChannel : EventChannel<TChannel>
            where TEvent : IEvent<TChannel>
        {
            GetEventChannel<TChannel>().Publish<TEvent>(@event);
        }
    }
}