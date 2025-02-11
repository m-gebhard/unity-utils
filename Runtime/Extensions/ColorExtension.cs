using UnityEngine;

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
}
