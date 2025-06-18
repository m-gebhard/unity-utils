using System;
using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for objects.
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Gets the type prefix of the object.
        /// </summary>
        /// <param name="obj">The object to get the type prefix for.</param>
        /// <returns>A string representing the type prefix of the object.</returns>
        private static string GetObjectTypePrefix(this object obj) => $"[{obj.GetType().Name}] ";

        /// <summary>
        /// Logs a message with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the message for.</param>
        /// <param name="message">The message to log.</param>
        public static void Log(this object obj, object message) =>
            Debug.Log($"{GetObjectTypePrefix(obj)} {message}");

        /// <summary>
        /// Logs multiple messages with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the message for.</param>
        /// <param name="messages">The messages to log.</param>
        public static void Log(this object obj, params object[] messages) =>
            Debug.Log($"{GetObjectTypePrefix(obj)} {string.Join("; ", messages)}");

        /// <summary>
        /// Logs a warning message with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the warning message for.</param>
        /// <param name="message">The warning message to log.</param>
        public static void LogWarning(this object obj, object message) =>
            Debug.LogWarning($"{GetObjectTypePrefix(obj)} {message}");

        /// <summary>
        /// Logs an error message with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the error message for.</param>
        /// <param name="message">The error message to log.</param>
        public static void LogError(this object obj, object message) =>
            Debug.LogError($"{GetObjectTypePrefix(obj)} {message}");

        /// <summary>
        /// Logs an exception with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the exception for.</param>
        /// <param name="exception">The exception to log.</param>
        public static void LogException(this object obj, Exception exception) =>
            Debug.LogException(exception);
    }
}