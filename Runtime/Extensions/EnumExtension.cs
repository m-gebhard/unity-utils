using System;
using Random = UnityEngine.Random;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extensions for enums.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Returns a random value from the specified enum type.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>A random value from the enum.</returns>
        public static T RandomValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }
    }
}