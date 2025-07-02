using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.UI
{
    /// <summary>
    /// Provides extension methods for RectTransforms.
    /// </summary>
    public static class RectTransformExtension
    {
        /// <summary>
        /// Rebuilds the layout of the object to fix potential sizing issues.
        /// </summary>
        /// <param name="rect">The RectTransform of object.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        public static IEnumerator RebuildLayout(this RectTransform rect)
        {
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }
}