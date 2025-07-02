using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for the GameObject class.
    /// </summary>
    public static class GameObjectExtension
    {
        /// <summary>
        /// Retrieves a component of the specified type from the GameObject.
        /// If the component does not exist, it adds a new one to the GameObject.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve or add.</typeparam>
        /// <param name="obj">The GameObject to retrieve or add the component to.</param>
        /// <returns>The existing or newly added component of the specified type.</returns>
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();
            return !component
                ? obj.AddComponent<T>()
                : component;
        }
    }
}