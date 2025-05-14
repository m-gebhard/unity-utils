using UnityEngine;

namespace UnityUtils.GameObjects
{
    /// <summary>
    /// Abstract base class for creating singleton MonoBehaviour instances.
    /// </summary>
    /// <typeparam name="T">Type of the singleton class.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance;

        /// <summary>
        /// Gets the singleton instance. If the instance is not found, it searches for it in the scene.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = FindFirstObjectByType<T>();

                return instance;
            }
        }

        /// <summary>
        /// Awake method to initialize the singleton instance.
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// A persistent singleton that is not destroyed on scene load.
    /// </summary>
    /// <typeparam name="T">Type of the singleton class.</typeparam>
    public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
        /// <summary>
        /// Awake method to call DontDestroyOnLoad on the object.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    /// <summary>
    /// An ephemeral singleton that does not persist across scene loads.
    /// </summary>
    /// <typeparam name="T">Type of the singleton class.</typeparam>
    public abstract class EphemeralSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
    }
}