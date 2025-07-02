using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for the Quaternion class.
    /// </summary>
    public static class QuaternionExtension
    {
        /// <summary>
        /// Returns a new Quaternion with the specified Euler angles applied.
        /// </summary>
        /// <param name="quaternion">The original Quaternion.</param>
        /// <param name="x">The x component of the Euler angles to apply. Defaults to 0 if null.</param>
        /// <param name="y">The y component of the Euler angles to apply. Defaults to 0 if null.</param>
        /// <param name="z">The z component of the Euler angles to apply. Defaults to 0 if null.</param>
        /// <returns>A new Quaternion with the specified Euler angles applied.</returns>
        public static Quaternion With(this Quaternion quaternion, float? x = null, float? y = null, float? z = null)
        {
            Vector3 eulerAngles = new(
                x ?? 0f,
                y ?? 0f,
                z ?? 0f
            );

            Quaternion offset = Quaternion.Euler(eulerAngles);
            return quaternion * offset;
        }
    }
}