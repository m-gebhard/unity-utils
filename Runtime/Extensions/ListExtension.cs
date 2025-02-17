using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for lists of floats and generic lists.
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// Linearly interpolates between elements in a list of floats based on a parameter t.
        /// </summary>
        /// <param name="list">The list of floats to interpolate.</param>
        /// <param name="t">The interpolation parameter, typically between 0 and 1.</param>
        /// <returns>The interpolated float value.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the list is empty.</exception>
        public static float Lerp(this List<float> list, float t)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("List<float> must not be empty.");
            }
            else if (list.Count == 1)
            {
                return list[0];
            }

            int startIndex = (int)(t * (list.Count - 1));
            int endIndex = startIndex + 1;

            float startValue = list[startIndex];
            float endValue = list[endIndex];

            float fraction = (t - (float)startIndex / (list.Count - 1)) * (list.Count - 1);

            return Mathf.Lerp(startValue, endValue, fraction);
        }

        /// <summary>
        /// Shuffles the elements of a list in place using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this List<T> list)
        {
            Random rng = new();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        /// <summary>
        /// Returns a random element from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to select a random element from.</param>
        /// <returns>A random element from the list.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the list is empty.</exception>
        public static T Random<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException($"List<{typeof(T)}> must not be empty.");
            }
            else if (list.Count == 1)
            {
                return list[0];
            }

            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}