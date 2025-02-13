using System.Text.RegularExpressions;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for Strings.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Swaps occurrences of two specified substrings within the given text.
        /// </summary>
        /// <param name="text">The original text where the swap will occur.</param>
        /// <param name="swapA">The first substring to swap.</param>
        /// <param name="swapB">The second substring to swap.</param>
        /// <returns>A new string with the specified substrings swapped.</returns>
        public static string Swap(this string text, string swapA, string swapB)
        {
            return Regex.Replace(text, Regex.Escape(swapA) + "|" + Regex.Escape(swapB),
                m => m.Value == swapA ? swapB : swapA);
        }
    }
}