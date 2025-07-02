using System;
using System.Collections;
using UnityEngine;

namespace UnityUtils.Data
{
    /// <summary>
    /// Utility class for different transitions of various types.
    /// </summary>
    public class FadeTransitions : MonoBehaviour
    {
        /// <summary>
        /// Generic coroutine to fade a value over time.
        /// </summary>
        /// <typeparam name="T">The type of the value to fade.</typeparam>
        /// <param name="getter">Function to get the current value.</param>
        /// <param name="setter">Action to set the new value.</param>
        /// <param name="targetValue">The target value to fade to.</param>
        /// <param name="lerpFunc">Function to interpolate between values.</param>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <returns>An IEnumerator for use in a coroutine.</returns>
        public static IEnumerator FadeValue<T>(
            Func<T> getter,
            Action<T> setter,
            T targetValue,
            Func<T, T, float, T> lerpFunc,
            float duration = 1f
        )
        {
            float elapsed = 0f;
            T startValue = getter();

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                T newValue = lerpFunc(startValue, targetValue, t);
                setter(newValue);

                yield return null;
            }

            setter(targetValue);
        }

        /// <summary>
        /// Coroutine to fade a material's color property over time.
        /// </summary>
        /// <param name="targetMaterial">The material to modify.</param>
        /// <param name="targetColor">The target color to fade to.</param>
        /// <param name="fieldName">The name of the color property to modify (default is "_EmissionColor").</param>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <returns>An IEnumerator for use in a coroutine.</returns>
        public static IEnumerator FadeMaterialColor(
            Material targetMaterial,
            Color targetColor,
            string fieldName = "_EmissionColor",
            float duration = 1f
        )
        {
            int propertyId = Shader.PropertyToID(fieldName);

            return FadeValue(
                () => targetMaterial.GetColor(propertyId),
                value => targetMaterial.SetColor(propertyId, value),
                targetColor,
                Color.Lerp,
                duration
            );
        }

        /// <summary>
        /// Coroutine to fade a light's intensity over time.
        /// </summary>
        /// <param name="targetLight">The light to modify.</param>
        /// <param name="targetIntensity">The target intensity to fade to.</param>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <returns>An IEnumerator for use in a coroutine.</returns>
        public static IEnumerator FadeLightIntensity(
            Light targetLight,
            float targetIntensity,
            float duration = 1f
        ) => FadeValue(
            () => targetLight.intensity,
            value => targetLight.intensity = value,
            targetIntensity,
            Mathf.Lerp,
            duration
        );

        /// <summary>
        /// Coroutine to fade a light's color over time.
        /// </summary>
        /// <param name="targetLight">The light to modify.</param>
        /// <param name="targetColor">The target color to fade to.</param>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <returns>An IEnumerator for use in a coroutine.</returns>
        public static IEnumerator FadeLightColor(
            Light targetLight,
            Color targetColor,
            float duration = 1f
        ) => FadeValue(
            () => targetLight.color,
            value => targetLight.color = value,
            targetColor,
            Color.Lerp,
            duration
        );
    }
}