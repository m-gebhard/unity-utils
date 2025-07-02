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

        /// <summary>
        /// Plays an AudioClip with random pitch and volume variations for added audio diversity.
        /// </summary>
        /// <param name="audioSource">The AudioSource to play the clip on.</param>
        /// <param name="clip">The AudioClip to play.</param>
        /// <param name="audioVolume">The base volume at which to play the clip. Defaults to 1.</param>
        /// <param name="maxPitchVariation">The maximum variation in pitch. Defaults to 0.1.</param>
        /// <param name="maxVolumeVariation">The maximum variation in volume. Defaults to 0.1.</param>
        public static void PlayOneShotWithVariance(
            this AudioSource audioSource,
            AudioClip clip,
            float audioVolume = 1f,
            float maxPitchVariation = 0.1f,
            float maxVolumeVariation = 0.1f)
        {
            float pitchBefore = audioSource.pitch;

            audioSource.pitch = Random.Range(1f - maxPitchVariation, 1f + maxPitchVariation);
            float volumeScale = Random.Range(1f - maxVolumeVariation, 1f + maxVolumeVariation);

            audioSource.PlayOneShot(clip, audioVolume * volumeScale);
            audioSource.pitch = pitchBefore;
        }
    }
}