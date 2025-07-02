using System.Globalization;
using UnityEngine;

namespace UnityUtils.Data
{
    /// <summary>
    /// Provides utility methods for formatting values.
    /// </summary>
    public static class Formatters
    {
        /// <summary>
        /// Formats a float value representing time in seconds to a string in the format "MM:SS:FF".
        /// </summary>
        /// <param name="time">The time value in seconds.</param>
        /// <returns>A formatted time string in the format "MM:SS:FF".</returns>
        public static string FormatFloatToTimeString(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

            return $"{minutes:D2}:{seconds:D2}:{milliseconds / 10:D2}";
        }

        /// <summary>
        /// Formats a double value to a USD currency string.
        /// </summary>
        /// <param name="value">The double value to format.</param>
        /// <returns>A formatted USD currency string.</returns>
        public static string FormatDoubleToUsdString(double value)
        {
            return value.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}