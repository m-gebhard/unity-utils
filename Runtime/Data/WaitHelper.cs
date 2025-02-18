using System.Collections.Generic;
using UnityEngine;

public static class WaitHelper
{
    /// <summary>
    /// A dictionary to cache WaitForSeconds instances.
    /// </summary>
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();

    /// <summary>
    /// Gets a WaitForEndOfFrame instance.
    /// </summary>
    public static WaitForEndOfFrame WaitForEndOfFrame { get; } = new();

    /// <summary>
    /// Gets a WaitForFixedUpdate instance.
    /// </summary>
    public static WaitForFixedUpdate WaitForFixedUpdate { get; } = new();

    /// <summary>
    /// Gets a cached WaitForSeconds instance for the specified duration.
    /// </summary>
    /// <param name="seconds">The duration in seconds.</param>
    /// <returns>A WaitForSeconds instance.</returns>
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!WaitDictionary.TryGetValue(seconds, out WaitForSeconds waitForSeconds))
        {
            waitForSeconds = new WaitForSeconds(seconds);
            WaitDictionary.Add(seconds, waitForSeconds);
        }

        return waitForSeconds;
    }
}