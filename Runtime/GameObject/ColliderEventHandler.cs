using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.GameObjects
{
    /// <summary>
    /// Handles runtime registration of collision and trigger events based on object tags.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class ColliderEventHandler : MonoBehaviour
    {
        /// <summary>
        /// Stores collision and trigger events by tag.
        /// </summary>
        private readonly Dictionary<string, ColliderEventEntry> registeredEvents = new();

        /// <summary>
        /// Registers a collider event by tag.
        /// </summary>
        protected void RegisterColliderEvent(ColliderEvent colliderEvent, bool isTriggerEvent = false)
        {
            if (!registeredEvents.TryGetValue(colliderEvent.Tag, out ColliderEventEntry entry))
            {
                entry = new ColliderEventEntry();
            }

            if (isTriggerEvent)
            {
                entry.TriggerEvent = colliderEvent;
            }
            else
            {
                entry.CollisionEvent = colliderEvent;
            }

            registeredEvents[colliderEvent.Tag] = entry;
        }

        /// <summary>
        /// Unregisters a collider event by a given tag.
        /// </summary>
        protected void UnregisterColliderEvent(string eventTag)
        {
            registeredEvents.Remove(eventTag);
        }

        /// <summary>
        /// Called when a trigger collider enters the trigger.
        /// </summary>
        /// <param name="other">The other collider involved in the trigger event.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (registeredEvents.TryGetValue(other.tag, out ColliderEventEntry entry))
            {
                entry.TriggerEvent.OnEnter?.Invoke(other.gameObject, null);
            }
        }

        /// <summary>
        /// Called when a trigger collider exits the trigger.
        /// </summary>
        /// <param name="other">The other collider involved in the trigger event.</param>
        private void OnTriggerExit(Collider other)
        {
            if (registeredEvents.TryGetValue(other.tag, out ColliderEventEntry entry))
            {
                entry.TriggerEvent.OnExit?.Invoke(other.gameObject, null);
            }
        }

        /// <summary>
        /// Called when a collision occurs.
        /// </summary>
        /// <param name="other">The collision data associated with the collision event.</param>
        private void OnCollisionEnter(Collision other)
        {
            if (registeredEvents.TryGetValue(other.gameObject.tag, out ColliderEventEntry entry))
            {
                entry.CollisionEvent.OnEnter?.Invoke(other.gameObject, other.contacts);
            }
        }

        /// <summary>
        /// Called when a collision ends.
        /// </summary>
        /// <param name="other">The collision data associated with the collision event.</param>
        private void OnCollisionExit(Collision other)
        {
            if (registeredEvents.TryGetValue(other.gameObject.tag, out ColliderEventEntry entry))
            {
                entry.CollisionEvent.OnExit?.Invoke(other.gameObject, other.contacts);
            }
        }
    }

    /// <summary>
    /// Struct representing a collider event.
    /// </summary>
    public struct ColliderEvent
    {
        public string Tag;
        public Action<GameObject, ContactPoint[]> OnEnter;
        public Action<GameObject, ContactPoint[]> OnExit;
    }

    /// <summary>
    /// Stores collision and trigger events.
    /// </summary>
    public struct ColliderEventEntry
    {
        public ColliderEvent CollisionEvent;
        public ColliderEvent TriggerEvent;
    }
}