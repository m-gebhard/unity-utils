using System;
using UnityEngine;

namespace UnityUtils.Data
{
    /// <summary>
    /// Represents a generic JSON object that can be serialized and deserialized using Unity's JsonUtility.
    /// The derived class is required to have the [Serializable] attribute.
    /// </summary>
    /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
    [Serializable]
    public abstract class JSONObject<T>
    {
        /// <summary>
        /// Serializes the current object to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the current object.</returns>
        public string ToJSON()
        {
            return JsonUtility.ToJson(this);
        }

        /// <summary>
        /// Deserializes the given JSON string to an object of type T.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>An object of type T deserialized from the JSON string.</returns>
        public T FromJSON(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}