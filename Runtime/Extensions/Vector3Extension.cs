using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for the Vector3 class.
    /// </summary>
    public static class Vector3Extension
    {
        /// <summary>
        /// Multiplies each component of the vector by the corresponding component of another vector.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="other">The vector to multiply with.</param>
        /// <returns>A new vector with each component multiplied by the corresponding component of the other vector.</returns>
        public static Vector3 Multiply(this Vector3 vector, Vector3 other)
        {
            return new Vector3(vector.x * other.x, vector.y * other.y, vector.z * other.z);
        }

        /// <summary>
        /// Multiplies each component of the vector by a scalar value.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="value">The scalar value to multiply with.</param>
        /// <returns>A new vector with each component multiplied by the scalar value.</returns>
        public static Vector3 Multiply(this Vector3 vector, float value)
        {
            return new Vector3(vector.x * value, vector.y * value, vector.z * value);
        }

        /// <summary>
        /// Divides each component of the vector by the corresponding component of another vector.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="other">The vector to divide by.</param>
        /// <returns>A new vector with each component divided by the corresponding component of the other vector, or the original component if the divisor is zero.</returns>
        public static Vector3 Divide(this Vector3 vector, Vector3 other)
        {
            return new Vector3(
                other.x != 0 ? vector.x / other.x : vector.x,
                other.y != 0 ? vector.y / other.y : vector.y,
                other.z != 0 ? vector.z / other.z : vector.z
            );
        }

        /// <summary>
        /// Divides each component of the vector by a scalar value.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="value">The scalar value to divide by.</param>
        /// <returns>A new vector with each component divided by the scalar value, or the original component if the divisor is zero.</returns>
        public static Vector3 Divide(this Vector3 vector, float value)
        {
            bool isZero = value == 0;
            return new Vector3(
                !isZero ? vector.x / value : vector.x,
                !isZero ? vector.y / value : vector.y,
                !isZero ? vector.z / value : vector.z
            );
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="other">The vector to calculate the distance to.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static float Distance(this Vector3 vector, Vector3 other)
        {
            return Vector3.Distance(vector, other);
        }
    }
}