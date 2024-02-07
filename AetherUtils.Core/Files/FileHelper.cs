using AetherUtils.Core.RegEx;
using System.Diagnostics;
using System.Text;

namespace AetherUtils.Core.Files
{
    /// <summary>
    /// Provides helper methods to manipulate files and folders on the Windows file system.
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
            DirectoryInfo? dInfo = Directory.GetParent(filePath);
            if (dInfo != null)
                Directory.CreateDirectory(dInfo.FullName);
        }

        /// <summary>
        /// Create a new file and write the specified content to it, using <see cref="Encoding.UTF32"/> encoding.
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
            File.WriteAllText(filePath, content, Encoding.UTF32);
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
        /// Open a binary file, read all bytes within to a byte array, and close the file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before attempting to open the file?</param>
        /// <returns>The contents of <paramref name="filePath"/> or an empty array if the file could not be opened.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static byte[] OpenNonTextFile(string filePath, bool expandPath = true)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            return File.ReadAllBytes(filePath);
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
        /// Asynchronously create a new file and write the specified content to it, using <see cref="Encoding.UTF32"/> encoding.
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
            await File.WriteAllTextAsync(filePath, content, Encoding.UTF32);
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
        /// <returns>The expanded path, or the original path if no expansion was necessary or an error occured.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="filePath"/> was <c>null</c> or empty.</exception>
        public static string ExpandPath(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            return filePath.Contains('%') ? Environment.ExpandEnvironmentVariables(filePath) : filePath;
        }

        /// <summary>
        /// Gets whether the specified path is a valid absolute file path on Windows.
        /// </summary>
        /// <param name="path">A path to check.</param>
        /// <exception cref="ArgumentException">If the <paramref name="path"/> was <c>null</c> or empty.</exception>
        public static bool IsValidPath(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            return RegexGenerator.PathRegex().IsMatch(path);
        }

        #endregion

        #region File Name and Path Manipulators

        /// <summary>
        /// Removes the platform-specific invalid file name characters from the specified file name.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>A string with the invalid characters removed from the filename.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="fileName"/> was <c>null</c> or empty.</exception>
        public static string RemoveInvalidFileNameChars(string fileName)
        {
            ArgumentException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
            
            var invalidFileChars = Path.GetInvalidFileNameChars();
            string? newFileName = fileName.Except(invalidFileChars).ToString();
            return newFileName ?? fileName;
        }

        /// <summary>
        /// Removes the platform-specific invalid path characters from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A string with the invalid characters removed from the path.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="path"/> was <c>null</c> or empty.</exception>
        public static string RemoveInvalidPathChars(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            
            var invalidPathChars = Path.GetInvalidPathChars();
            string? newPath = path.Except(invalidPathChars).ToString();
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
        #endregion
    }
}
