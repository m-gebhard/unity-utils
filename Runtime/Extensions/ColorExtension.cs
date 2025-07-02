using System;
using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for the Color class.
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// Adjusts the saturation and brightness of the given color by the specified intensity.
        /// </summary>
        /// <param name="color">The original color to adjust.</param>
        /// <param name="intensity">The intensity by which to adjust the saturation and value. Positive values increase, negative values decrease.</param>
        /// <returns>A new color with adjusted saturation and value.</returns>
        public static Color AdjustSaturation(this Color color, float intensity)
        {
            // Convert color to HSV
            Color.RGBToHSV(color, out float h, out float s, out float v);

            // Adjust saturation and brightness while keeping hue constant
            s += intensity; // Increase saturation
            v += intensity; // Increase value (brightness)

            // Clamp values to the range [0, 1]
            s = Mathf.Clamp01(s);
            v = Mathf.Clamp01(v);

            // Convert back to RGB
            Color adjustedColor = Color.HSVToRGB(h, s, v);

            return adjustedColor;
        }

        /// <summary>
        /// Converts the color to a hexadecimal string.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>A hexadecimal string representation of the color.</returns>
        public static string ToHex(this Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }

        /// <summary>
        /// Converts a hexadecimal string to a Color.
        /// </summary>
        /// <param name="hex">The hexadecimal string to convert.</param>
        /// <returns>The Color represented by the hexadecimal string.</returns>
        /// <exception cref="Exception">Thrown when the hex string is in an invalid format.</exception>
        public static Color FromHex(this string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                throw new Exception("Invalid hex color format.");
            }

            return color;
        }

        /// <summary>
        /// Creates a new color with the same RGB values but a modified alpha value.
        /// </summary>
        /// <param name="color">The original color.</param>
        /// <param name="alpha">The new alpha value.</param>
        /// <returns>A new color with the specified alpha value.</returns>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}