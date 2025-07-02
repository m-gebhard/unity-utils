using System;
using UnityEngine;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for objects to enhance logging functionality.
    /// </summary>
    public static class ObjectExtension
    {
        private const string LogValueSeparator = ";";

        /// <summary>
        /// Gets the type prefix of the object.
        /// </summary>
        /// <param name="obj">The object to get the type prefix for.</param>
        /// <returns>A string representing the type prefix of the object.</returns>
        private static string GetObjectTypePrefix(this object obj) => $"[{obj.GetType().Name}] ";

        /// <summary>
        /// Outputs multiple messages using the specified logging function.
        /// </summary>
        /// <param name="obj">The object to log the messages for.</param>
        /// <param name="logFunction">The logging function to use (e.g., Debug.Log).</param>
        /// <param name="messages">The messages to log.</param>
        private static void OutputMultiple(this object obj, Action<string> logFunction, params object[] messages) =>
            logFunction($"{GetObjectTypePrefix(obj)} {string.Join($"{LogValueSeparator} ", messages)}");

        #region Log Methods

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
        /// <param name="obj">The object to log the messages for.</param>
        /// <param name="messages">The messages to log.</param>
        public static void Log(this object obj, params object[] messages) => OutputMultiple(obj, Debug.Log, messages);

        /// <summary>
        /// Logs a warning message with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the warning message for.</param>
        /// <param name="message">The warning message to log.</param>
        public static void LogWarning(this object obj, object message) =>
            Debug.LogWarning($"{GetObjectTypePrefix(obj)} {message}");

        /// <summary>
        /// Logs multiple warning messages with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the warning messages for.</param>
        /// <param name="messages">The warning messages to log.</param>
        public static void LogWarning(this object obj, params object[] messages) =>
            OutputMultiple(obj, Debug.LogWarning, messages);

        /// <summary>
        /// Logs an error message with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the error message for.</param>
        /// <param name="message">The error message to log.</param>
        public static void LogError(this object obj, object message) =>
            Debug.LogError($"{GetObjectTypePrefix(obj)} {message}");

        /// <summary>
        /// Logs multiple error messages with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the error messages for.</param>
        /// <param name="messages">The error messages to log.</param>
        public static void LogError(this object obj, params object[] messages) =>
            OutputMultiple(obj, Debug.LogError, messages);

        /// <summary>
        /// Logs an exception with the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the exception for.</param>
        /// <param name="exception">The exception to log.</param>
        public static void LogException(this object obj, Exception exception) =>
            Debug.LogException(exception);

        /// <summary>
        /// Logs an exception with a custom message and the object's type prefix.
        /// </summary>
        /// <param name="obj">The object to log the exception for.</param>
        /// <param name="message">The custom message to include with the exception.</param>
        /// <param name="exception">The exception to log.</param>
        public static void LogException(this object obj, string message, Exception exception) =>
            Debug.LogException(new Exception($"{GetObjectTypePrefix(obj)} {message}", exception));

        #endregion
    }
}