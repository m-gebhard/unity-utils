using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Sorts the elements of the list in place using the specified comparison.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparison">The comparison to use for sorting the elements.</param>
        /// <returns>The sorted list.</returns>
        public static List<T> SortInPlace<T>(this List<T> list, Comparison<T> comparison)
        {
            if (list == null || list.Count <= 1)
            {
                return list;
            }

            list.Sort(comparison);
            return list;
        }

        /// <summary>
        /// Shuffles the elements of a list in place using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static List<T> Shuffle<T>(this List<T> list)
        {
            Random rng = new();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }

            return list;
        }

        /// <summary>
        /// Creates a shallow copy of the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to clone.</param>
        /// <returns>A new list containing the same elements as the original list.</returns>
        public static List<T> Clone<T>(this List<T> list)
        {
            return new List<T>(list);
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

        /// <summary>
        /// Returns the next element in the list after the specified item.
        /// If the item is not found, returns the first element if the list is not empty, otherwise returns the default value for the type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to search.</param>
        /// <param name="item">The item to find the next element of.</param>
        /// <returns>The next element in the list after the specified item, or the first element if the item is not found and the list is not empty, otherwise the default value for the type.</returns>
        public static T Next<T>(this List<T> list, T item)
        {
            int index = list.IndexOf(item);

            if (index == -1)
            {
                return list.Count > 0
                    ? list[0]
                    : default;
            }

            int nextIndex = (index + 1) % list.Count;
            return list[nextIndex];
        }

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
        /// Finds the nearest transform in the list to the specified transform.
        /// </summary>
        /// <param name="transformList">The list of transforms to search.</param>
        /// <param name="compareTransform">The transform to compare against.</param>
        /// <returns>The nearest transform to the specified transform.</returns>
        public static Transform Nearest(this List<Transform> transformList, Transform compareTransform)
        {
            Transform nearest = null;
            float shortest = float.MaxValue;

            foreach (Transform t in transformList)
            {
                float dist = Vector3.Distance(compareTransform.position, t.position);

                if (dist < shortest)
                {
                    shortest = dist;
                    nearest = t;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Finds the furthest transform in the list from the specified transform.
        /// </summary>
        /// <param name="transformList">The list of transforms to search.</param>
        /// <param name="compareTransform">The transform to compare against.</param>
        /// <returns>The furthest transform from the specified transform.</returns>
        public static Transform Furthest(this List<Transform> transformList, Transform compareTransform)
        {
            Transform furthest = null;
            float furthestDist = 0;

            foreach (Transform t in transformList)
            {
                float dist = Vector3.Distance(compareTransform.position, t.position);

                if (dist > furthestDist)
                {
                    furthestDist = dist;
                    furthest = t;
                }
            }

            return furthest;
        }

        /// <summary>
        /// Calculates the center point of a list of Vector3s.
        /// </summary>
        /// <param name="list">The list of Vector3s to calculate the center point of.</param>
        /// <returns>The center point of the list of Vector3s.</returns>
        public static Vector3 Center(this List<Vector3> list)
        {
            Vector3 average = list.Aggregate(Vector3.zero, (current, v) => current + v);
            average /= list.Count;

            return average;
        }
    }
}