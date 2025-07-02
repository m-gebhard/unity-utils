using System;
using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for the LayerMask class.
    /// </summary>
    public static class LayerMaskExtension
    {
        /// <summary>
        /// Creates a LayerMask from an array of layer indices.
        /// </summary>
        /// <param name="layerIndices">An array of layer indices.</param>
        /// <returns>A LayerMask representing the specified layers.</returns>
        /// <exception cref="Exception">Thrown when a layer index is out of the valid range (0-31).</exception>
        public static LayerMask CreateFromIndices(params int[] layerIndices)
        {
            int areaMask = 0;

            foreach (int layerIndex in layerIndices)
            {
                if (layerIndex is >= 0 and < 32)
                {
                    areaMask |= 1 << layerIndex;
                }
                else
                {
                    throw new Exception($"Layer index {layerIndex} is out of range (0-31)");
                }
            }

            return areaMask;
        }
    }
}