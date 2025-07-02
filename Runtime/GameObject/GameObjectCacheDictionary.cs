using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils.GameObjects;

/// <summary>
/// Abstract class representing a cache dictionary for GameObjects.
/// </summary>
/// <typeparam name="T">The type of the value to be cached.</typeparam>
public abstract class GameObjectCacheDictionary<T> : EphemeralSingleton<GameObjectCacheDictionary<T>>
{
    /// <summary>
    /// Dictionary to store cached values by GameObject.
    /// </summary>
    private readonly Dictionary<GameObject, T> cached = new();

    /// <summary>
    /// Gets the value associated with the specified GameObject key, or adds a new value if the key does not exist.
    /// </summary>
    /// <param name="key">The GameObject key.</param>
    /// <param name="valueFactory">The function to create a new value if the key does not exist.</param>
    /// <param name="discardNullValue">Whether to discard the value if the given function returns null,
    /// otherwise it will cache the null value</param>
    /// <returns>The value associated with the specified key.</returns>
    public T GetOrAdd(
        GameObject key,
        Func<GameObject, T> valueFactory,
        bool discardNullValue = true
    )
    {
        if (!cached.TryGetValue(key, out T value))
        {
            value = valueFactory(key);

            if (value != null || !discardNullValue)
            {
                cached[key] = value;
            }
        }

        return value;
    }

    /// <summary>
    /// Adds or updates a cached item with the specified GameObject key and value.
    /// </summary>
    /// <param name="key">The GameObject key.</param>
    /// <param name="value">The value to be cached.</param>
    public void AddCachedItem(GameObject key, T value)
    {
        cached[key] = value;
    }

    /// <summary>
    /// Removes the cached item associated with the specified GameObject key.
    /// </summary>
    /// <param name="key">The GameObject key.</param>
    public void RemoveCachedItem(GameObject key)
    {
        cached.Remove(key);
    }

    /// <summary>
    /// Tries to get the value associated with the specified GameObject key.
    /// </summary>
    /// <param name="key">The GameObject key.</param>
    /// <param name="value">The associated value if found; otherwise, the default value.</param>
    /// <returns>true if the key was found; otherwise, false.</returns>
    public bool TryGetValue(GameObject key, out T value)
    {
        return cached.TryGetValue(key, out value);
    }

    /// <summary>
    /// Determines whether the cache contains the specified GameObject key.
    /// </summary>
    /// <param name="key">The GameObject key.</param>
    /// <returns>true if the cache contains the specified key; otherwise, false.</returns>
    public bool ContainsKey(GameObject key)
    {
        return cached.ContainsKey(key);
    }

    /// <summary>
    /// Clears all cached items.
    /// </summary>
    public void Clear()
    {
        cached.Clear();
    }
}