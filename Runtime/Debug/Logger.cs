using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils.Data;
using UnityUtils.GameObjects;

namespace UnityUtils.Debugging
{
    /// <summary>
    /// A logger class that persists log entries and saves them to a file upon application quit.
    /// </summary>
    public class Logger : PersistentSingleton<Logger>
    {
        /// <summary>
        /// The prefix for the log file name.
        /// </summary>
        [SerializeField] private string logFilePrefix = "log_";

        /// <summary>
        /// A list of words to ignore in log entries.
        /// </summary>
        [SerializeField] private List<string> ignoredWords = new();

        /// <summary>
        /// Whether to append stack traces to log entries.
        /// </summary>
        [SerializeField] private bool appendStackToLogs = false;

        /// <summary>
        /// A list of log entries.
        /// </summary>
        private static readonly List<LogEntry> entries = new();

        /// <summary>
        /// The start time of the logger.
        /// </summary>
        private DateTime startTime;

        /// <summary>
        /// The name of the log file.
        /// </summary>
        private string logName = "";

        /// <summary>
        /// Gets the complete log as a string.
        /// </summary>
        public string Log => GetLogFileHead() + "\n" + CreateReadableLog();

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            startTime = DateTime.Now;
            logName = $"{logFilePrefix}{startTime:yyyyMMdd\\THHmmss}";

            Application.logMessageReceived += OnLogEntry;
        }

        /// <summary>
        /// Cleans up the logger.
        /// </summary>
        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLogEntry;
        }

        /// <summary>
        /// Saves the log to a file when the application quits.
        /// </summary>
        private void OnApplicationQuit()
        {
            SaveLogToFile();
        }

        /// <summary>
        /// Saves the log to a file.
        /// </summary>
        private void SaveLogToFile()
        {
            var content = GetLogFileHead() + "\n" + CreateReadableLog();

            if (!FileManager.WriteToFile($"logs/{logName}.log", content))
            {
                Debug.LogError("Error saving logfile");
            }
        }

        /// <summary>
        /// Gets the header for the log file.
        /// </summary>
        /// <returns>A string containing the log file header.</returns>
        private string GetLogFileHead()
        {
            return $"Application Version: {Application.version} - Time: {startTime}";
        }

        /// <summary>
        /// Handles a new log entry.
        /// </summary>
        /// <param name="logString">The log message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="type">The type of log message.</param>
        private void OnLogEntry(string logString, string stackTrace, LogType type)
        {
            if (ContainsIgnoredWord(logString) || ContainsIgnoredWord(stackTrace)) return;

            var stack = type != LogType.Log || appendStackToLogs ? stackTrace : "";

            entries.Add(new LogEntry(type.ToString(), DateTime.Now, logString, stack));
        }

        /// <summary>
        /// Creates a readable log string from the log entries.
        /// </summary>
        /// <returns>A string containing the readable log.</returns>
        private static string CreateReadableLog()
        {
            string log = "";

            foreach (LogEntry entry in entries)
            {
                log += $"[{entry.dateTime}] {entry.type}: {entry.logString}\n";

                if (entry.stackTrace != "")
                {
                    log += $"{entry.stackTrace}\n";
                }
            }

            return log;
        }

        /// <summary>
        /// Checks if the given text contains any ignored words.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <returns>True if the text contains any ignored words, otherwise false.</returns>
        private bool ContainsIgnoredWord(string text)
        {
            return ignoredWords.Any(text.Contains);
        }
    }

    /// <summary>
    /// Represents a log entry.
    /// </summary>
    [Serializable]
    public class LogEntry
    {
        public string type;
        public DateTime dateTime;
        public string logString;
        public string stackTrace;

        public LogEntry(string logType, DateTime time, string log, string stack)
        {
            type = logType;
            dateTime = time;
            logString = log;
            stackTrace = stack;
        }
    }
}