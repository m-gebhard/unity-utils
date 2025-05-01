using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.GameObjects
{
    /// <summary>
    /// A generic object pool for managing reusable objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of objects to pool, which must be a Component.</typeparam>
    public abstract class GenericObjectPool<T> : EphemeralSingleton<GenericObjectPool<T>> where T : Component
    {
        /// <summary>
        /// The prefab to instantiate for the pool.
        /// </summary>
        [SerializeField] private T prefab;

        /// <summary>
        /// The number of objects to prewarm in the pool.
        /// </summary>
        [SerializeField] private int prewarmCount = 0;

        /// <summary>
        /// Whether to awake objects immediately upon creation.
        /// </summary>
        [SerializeField] private bool awakeObjects = false;

        /// <summary>
        /// The queue of available objects in the pool.
        /// </summary>
        private readonly Queue<T> objects = new();

        /// <summary>
        /// Initializes the pool and prewarms objects if specified.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (prefab == null)
            {
                Debug.LogError($"Pool {GetType()} has no prefab assigned!");
                return;
            }

            if (prewarmCount > 0)
            {
                AddObjects(prewarmCount);
            }
        }

        /// <summary>
        /// Gets an object from the pool, creating one if necessary.
        /// </summary>
        /// <returns>An object of type T from the pool.</returns>
        public T Get()
        {
            if (objects.Count == 0)
                AddObjects(1, true);
            return objects.Dequeue();
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        /// <param name="objectToReturn">The object to return to the pool.</param>
        public void ReturnToPool(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            objects.Enqueue(objectToReturn);
        }

        /// <summary>
        /// Adds a specified number of objects to the pool.
        /// </summary>
        /// <param name="count">The number of objects to add.</param>
        /// <param name="directAccess">Whether the objects should be directly accessible.</param>
        private void AddObjects(int count, bool directAccess = false)
        {
            for (var i = 0; i < count; i++)
            {
                T newObject = Instantiate(prefab, transform);

                if (awakeObjects && !directAccess)
                {
                    newObject.gameObject.SetActive(true);
                    StartCoroutine(DisableAfterFrame(newObject));
                }
                else
                {
                    newObject.gameObject.SetActive(false);
                    objects.Enqueue(newObject);
                }
            }
        }

        /// <summary>
        /// Disables a newly created object after one frame.
        /// </summary>
        /// <param name="newObject">The object to disable.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator DisableAfterFrame(T newObject)
        {
            yield return null;

            newObject.gameObject.SetActive(false);
            objects.Enqueue(newObject);
        }
    }
}