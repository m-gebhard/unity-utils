using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityUtils.Data
{
    /// <summary>
    /// Provides methods for file management, including reading, writing, and deleting files.
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Gets the full path for the specified filename.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <returns>The full path of the file.</returns>
        private static string GetFullPath(string filename) =>
            Path.Combine(Application.persistentDataPath, filename);

        /// <summary>
        /// Writes the specified content to a file.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <param name="content">The content to write to the file.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        public static bool WriteToFile(string filename, string content)
        {
            try
            {
                var fullPath = GetFullPath(filename);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllText(fullPath, content);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {filename}: {e}");
                return false;
            }
        }

        /// <summary>
        /// Writes the specified byte array to a file.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <param name="data">The byte array to write to the file.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        public static bool WriteToFile(string filename, byte[] data)
        {
            try
            {
                var fullPath = GetFullPath(filename);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllBytes(fullPath, data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {filename}: {e}");
                return false;
            }
        }

        /// <summary>
        /// Loads the content of a file into a string.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <param name="result">The content of the file.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        public static bool LoadFromFile(string filename, out string result)
        {
            try
            {
                result = File.ReadAllText(GetFullPath(filename));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to read {filename}: {e}");
                result = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        public static bool DeleteFile(string filename)
        {
            try
            {
                File.Delete(GetFullPath(filename));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete {filename}: {e}");
                return false;
            }
        }

        /// <summary>
        /// Deletes all files in the specified folder.
        /// </summary>
        /// <param name="folderPath">The path of the folder.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        public static bool DeleteFilesFromFolder(string folderPath)
        {
            try
            {
                var fullFolderPath = GetFullPath(folderPath);
                if (!Directory.Exists(fullFolderPath)) return false;

                foreach (var file in Directory.GetFiles(fullFolderPath))
                    File.Delete(file);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete files in {folderPath}: {e}");
                return false;
            }
        }

        /// <summary>
        /// Gets the names of all files in the specified directory.
        /// </summary>
        /// <param name="folderPath">The path of the folder.</param>
        /// <param name="createIfNotExist">Whether to create the directory if it does not exist.</param>
        /// <returns>A list of file names in the directory.</returns>
        public static List<string> GetFileNamesInDirectory(string folderPath, bool createIfNotExist = false)
        {
            var fullPath = GetFullPath(folderPath);
            if (!Directory.Exists(fullPath))
            {
                if (createIfNotExist) Directory.CreateDirectory(fullPath);
                return new List<string>();
            }

            try
            {
                var files = new DirectoryInfo(fullPath).GetFiles();
                return new List<string>(Array.ConvertAll(files, f => f.Name));
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to list files in {folderPath}: {e}");
                return new List<string>();
            }
        }
    }
}