using System.Collections;
using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for Audio sources.
    /// </summary>
    public static class AudioSourceExtension
    {
        /// <summary>
        /// Fades the volume of the given AudioSource from a start volume to a target volume over a specified duration.
        /// </summary>
        /// <param name="audioSource">The AudioSource to fade.</param>
        /// <param name="duration">The duration over which to fade the volume.</param>
        /// <param name="targetVolume">The target volume to reach at the end of the fade.</param>
        /// <param name="startVolume">The starting volume. Defaults to 0.</param>
        /// <returns>An IEnumerator that can be used to run the fade operation in a coroutine.</returns>
        public static IEnumerator FadeVolume(
            this AudioSource audioSource,
            float duration,
            float targetVolume,
            float? startVolume = null)
        {
            float initialVolume = startVolume ?? audioSource.volume;
            float currentTime = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, currentTime / duration);

                yield return null;
            }
        }
    }
}