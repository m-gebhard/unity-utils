using System.Collections;
using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for the Transform class.
    /// </summary>
    public static class TransformExtension
    {
        /// <summary>
        /// Smoothly moves a Transform to match the position and rotation of a target Transform over a specified duration.
        /// </summary>
        /// <param name="target">The Transform to move.</param>
        /// <param name="destination">The target Transform to move towards.</param>
        /// <param name="duration">The duration of the movement in seconds. Defaults to 0.5 seconds.</param>
        /// <returns>An IEnumerator that can be used in a coroutine to perform the movement.</returns>
        public static IEnumerator MoveToTargetTransform(
            this Transform target,
            Transform destination,
            float duration = 0.5f
        )
        {
            float elapsed = 0f;

            Vector3 startPos = target.position;
            Quaternion startRot = target.rotation;

            Vector3 endPos = destination.position;
            Quaternion endRot = destination.rotation;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsed / duration);

                target.position = Vector3.Lerp(startPos, endPos, t);
                target.rotation = Quaternion.Slerp(startRot, endRot, t);

                yield return null;
            }

            target.position = endPos;
            target.rotation = endRot;
        }
    }
}