﻿using System.Diagnostics;
using System.Security;
using AetherUtils.Core.RegEx;
using System.Text;

namespace AetherUtils.Core.Files
{
    /// <summary>
    /// Provides methods to manipulate files and folders on the Windows file system.
    /// </summary>
    public static class FileHelper
    {
        #region Create File/Folder Helpers
        /// <summary>
        /// Create directories along the specified file path if they don't already exist.
        /// </summary>
        /// <param name="filePath">The path to create directories on.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating directories?</param>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static void CreateDirectories(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            var dInfo = Directory.GetParent(filePath);
            if (dInfo != null)
                Directory.CreateDirectory(dInfo.FullName);
        }

        /// <summary>
        /// Create a new file and write the specified content to it.
        /// <para>If the file already exists, it is overwritten.</para>
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="content">The text to save to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static void SaveFile(string filePath, string content, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Open a text file, read all text within to a string, and close the file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before attempting to open the file?</param>
        /// <returns>The contents of <paramref name="filePath"/> or <see cref="string.Empty"/> if the file could not be opened.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static string OpenFile(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Open a text file, read all text within asynchronously to a string, and close the file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before attempting to open the file?</param>
        /// <returns>A <see cref="Task{TResult}"/> which contains the <see cref="string"/> contents upon completion.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static async Task<string> OpenFileAsync(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            filePath = expandPath ? ExpandPath(filePath) : filePath;
            return await File.ReadAllTextAsync(filePath);
        }

        /// <summary>
        /// Open a binary file, read all bytes within to a <see cref="byte"/> array, and close the file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before attempting to open the file?</param>
        /// <returns>The contents of <paramref name="filePath"/> as a <see cref="byte"/> array.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static byte[] OpenNonTextFile(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Open a binary file asynchronously, read all bytes within to a <see cref="byte"/> array, and close the file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before attempting to open the file?</param>
        /// <returns>A <see cref="Task{TResult}"/> which contains the <see cref="byte"/> array upon completion.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static async Task<byte[]> OpenNonTextFileAsync(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            filePath = expandPath ? ExpandPath(filePath) : filePath;
            return await File.ReadAllBytesAsync(filePath);
        }

        /// <summary>
        /// Create a new file and write the specified content to it, using the specified encoding. 
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath">The path of the file to save.</param>
        /// <param name="content">The text to save to the file.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to use when saving the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static void SaveFile(string filePath, string content, Encoding encoding, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            File.WriteAllText(filePath, content, encoding);
        }

        /// <summary>
        /// Create a new file and write the specified <see cref="byte"/> array to it.
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath">The path of the file to save.</param>
        /// <param name="contents">The <see cref="byte"/>s to save to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static void SaveFile(string filePath, byte[] contents, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            File.WriteAllBytes(filePath, contents);
        }

        /// <summary>
        /// Asynchronously create a new file and write the specified content to it.
        /// <para>If the file exists, it is overwritten.</para>
        /// </summary>
        /// <param name="filePath">The path of the file to save.</param>
        /// <param name="content">The text to save to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static async void SaveFileAsync(string filePath, string content, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            await File.WriteAllTextAsync(filePath, content);
        }

        /// <summary>
        /// Asynchronously create a new file and write the specified content to it, using the specified encoding.
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath">The path of the file to save.</param>
        /// <param name="content">The text to save to the file.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to use when saving the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static async void SaveFileAsync(string filePath, string content, Encoding encoding, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            await File.WriteAllTextAsync(filePath, content, encoding);
        }

        /// <summary>
        /// Asynchronously create a new file and write the specified <see cref="byte"/> array to it.
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath">The path of the file to save.</param>
        /// <param name="content">The <see cref="byte"/>s to save to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static async void SaveFileAsync(string filePath, byte[] content, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            await File.WriteAllBytesAsync(filePath, content);
        }

        #endregion

        #region Path Helpers
        /// <summary>
        /// Expand a file path containing environment path variables, if possible.
        /// </summary>
        /// <param name="filePath">The path to expand.</param>
        /// <returns>The expanded path, or the original path if no expansion was necessary.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static string ExpandPath(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            return filePath.Contains('%') ? Environment.ExpandEnvironmentVariables(filePath) : filePath;
        }

        /// <summary>
        /// Get whether the specified <paramref name="path"/> is a valid absolute file path on Windows.
        /// </summary>
        /// <param name="path">A path to check.</param>
        /// <param name="expandPath">Should the <paramref name="path"/> be expanded before checking?</param>
        /// <exception cref="ArgumentException">If the <paramref name="path"/> was <c>null</c> or empty.</exception>
        public static bool IsValidPath(string path, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            path = expandPath ? ExpandPath(path) : path;
            return RegexGenerator.PathRegex().IsMatch(path);
        }
        
        /// <summary>
        /// Enumerates directories in the specified path, handling restricted access and other exceptions.
        /// </summary>
        /// <param name="path">The directory path to search for directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. Defaults to "*".</param>
        /// <param name="searchOption">Specifies whether to search the current directory, or all subdirectories. Defaults to <see cref="SearchOption.TopDirectoryOnly"/>.</param>
        /// <returns>An enumerable collection of directory names in the specified directory.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="searchOption"/> is not a valid <see cref="SearchOption"/> value.</exception>
        public static IEnumerable<string> SafeEnumerateDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var directories = new List<string>();
            try
            {
                directories.AddRange(Directory.EnumerateDirectories(path, searchPattern, searchOption));
            }
            catch (UnauthorizedAccessException) {}
            catch (PathTooLongException) {}
            catch (DirectoryNotFoundException){}
            catch (SecurityException){}
            catch (IOException) {}

            return directories;
        }

        /// <summary>
        /// Enumerates files in the specified path, handling restricted access and other exceptions.
        /// </summary>
        /// <param name="path">The directory path to search for files.</param>
        /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>. Defaults to "*.*".</param>
        /// <param name="searchOption">Specifies whether to search the current directory, or all subdirectories. Defaults to <see cref="SearchOption.TopDirectoryOnly"/>.</param>
        /// <returns>An enumerable collection of file names in the specified directory.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="searchOption"/> is not a valid <see cref="SearchOption"/> value.</exception>
        public static IEnumerable<string> SafeEnumerateFiles(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var files = new List<string>();
            try
            {
                files.AddRange(Directory.EnumerateFiles(path, searchPattern, searchOption));
            }
            catch (UnauthorizedAccessException) {}
            catch (PathTooLongException) {}
            catch (DirectoryNotFoundException){}
            catch (SecurityException){}
            catch (IOException) {}

            return files;
        }

        #endregion

        #region File Name and Path Manipulators

        /// <summary>
        /// Remove the platform-specific invalid file name characters from the specified file name.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>A string with the invalid characters removed from the filename.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="fileName"/> was <c>null</c> or empty.</exception>
        public static string RemoveInvalidFileNameChars(string fileName)
        {
            ArgumentException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
            
            var invalidFileChars = Path.GetInvalidFileNameChars();
            var newFileName = fileName.Except(invalidFileChars).ToString();
            return newFileName ?? fileName;
        }

        /// <summary>
        /// Remove the platform-specific invalid path characters from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A string with the invalid characters removed from the path.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="path"/> was <c>null</c> or empty.</exception>
        public static string RemoveInvalidPathChars(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            
            var invalidPathChars = Path.GetInvalidPathChars();
            var newPath = path.Except(invalidPathChars).ToString();
            return newPath ?? path;
        }
        #endregion

        #region Exist Helpers
        /// <summary>
        /// Check if a file exists on the file system.
        /// </summary>
        /// <param name="filePath">The path to a file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before checking?</param>
        /// <returns><c>true</c> if the file exists; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static bool DoesFileExist(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            return File.Exists(filePath);
        }

        /// <summary>
        /// Check if a folder exists on the file system.
        /// </summary>
        /// <param name="folderPath">The path to a folder.</param>
        /// <param name="expandPath">Should the <paramref name="folderPath"/> be expanded before checking?</param>
        /// <returns><c>true</c> if the folder exists; <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="folderPath"/> was <c>null</c> or empty.</exception>
        public static bool DoesFolderExist(string folderPath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(folderPath, nameof(folderPath));

            folderPath = expandPath ? ExpandPath(folderPath) : folderPath;
            return Directory.Exists(folderPath);
        }

        /// <summary>
        /// Get the extension component, including the <c>.</c>, of a file name specified by <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The path to a file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before accessing?</param>
        /// <returns>The file extension, including the <c>.</c>.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static string GetExtension(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// Delete the file, if it exists, specified by <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">The path to a file to delete.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before accessing?</param>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static void DeleteFile(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            if (DoesFileExist(filePath))
                File.Delete(filePath);
        }
        #endregion
    }
}
