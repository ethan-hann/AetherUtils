using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static void CreateDirectories(string filePath, bool expandPath = true)
        {
            try
            {
                filePath = expandPath ? ExpandPath(filePath) : filePath;

                DirectoryInfo? dInfo = Directory.GetParent(filePath);
                if (dInfo != null)
                    Directory.CreateDirectory(dInfo.FullName);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        /// <summary>
        /// Create a new file and write the specified content to it. 
        /// If the file already exists, it is overwritten.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        public static void SaveFile(string filePath, string content, bool expandPath = true)
        {
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            try
            {
                File.WriteAllText(filePath, content);
            } catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        /// <summary>
        /// Open a text file, read all text within to a string, and close the file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before attempting to open the file?</param>
        /// <returns>The contents of <paramref name="filePath"/> or <see cref="string.Empty"/> if the file could not be opened.</returns>
        public static string OpenFile(string filePath, bool expandPath = true)
        {
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); return string.Empty; }
        }

        /// <summary>
        /// Open a binary file, read all bytes within to a byte array, and close the file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before attempting to open the file?</param>
        /// <returns>The contents of <paramref name="filePath"/> or an empty array if the file could not be opened.</returns>
        public static byte[] OpenNonTextFile(string filePath, bool expandPath = true)
        {
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); return []; }
        }

        /// <summary>
        /// Create a new file and write the specified content to it, using the specified encoding. 
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        public static void SaveFile(string filePath, string content, Encoding encoding, bool expandPath = true)
        {
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            try
            {
                File.WriteAllText(filePath, content, encoding);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        /// <summary>
        /// Create a new file and write the specified <see cref="byte"/> array to it.
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="contents"></param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        public static void SaveFile(string filePath, byte[] contents, bool expandPath = true)
        {
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            try
            {
                File.WriteAllBytes(filePath, contents);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        /// <summary>
        /// Asynchronously create a new file and write the specified content to it.
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        public static async void SaveFileAsync(string filePath, string content, bool expandPath = true)
        {
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            try
            {
                await File.WriteAllTextAsync(filePath, content);
            } catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        /// <summary>
        /// Asynchronously create a new file and write the specified content to it, using the specified encoding.
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        public static async void SaveFileAsync(string filePath, string content, Encoding encoding,  bool expandPath = true)
        {
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            try
            {
                await File.WriteAllTextAsync(filePath, content, encoding);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        /// <summary>
        /// Asynchronously create a new file and write the specified <see cref="byte"/> array to it.
        /// If the file exists, it is overwritten.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before creating file?</param>
        public static async void SaveFileAsync(string filePath, byte[] content, bool expandPath = true)
        {
            filePath = expandPath ? ExpandPath(filePath) : filePath;
            CreateDirectories(filePath, false);
            try
            {
                await File.WriteAllBytesAsync(filePath, content);
            } catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        #endregion

        #region Path Helpers
        /// <summary>
        /// Expand a file path containing environment path variables, if possible.
        /// </summary>
        /// <param name="filePath">The path to expand.</param>
        /// <returns>The expanded path, or the original path if no expansion was necessary or an error occured.</returns>
        public static string ExpandPath(string filePath)
        {
            try
            {
                //Expand environment path variables, if present in the file path string.
                if (filePath.Contains('%'))
                    return Environment.ExpandEnvironmentVariables(filePath);
                return filePath;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); return filePath; }
        }

        /// <summary>
        /// Try to expand a file path containing environment path variables (%), if possible.
        /// </summary>
        /// <param name="path">The path to expand.</param>
        /// <param name="expandedPath">The expanded path, or the original path if no expansion was necessary.</param>
        /// <returns><c>true</c> if expansion was successful; <c>false</c> otherwise.</returns>
        public static bool TryExpandPath(string path, out string expandedPath)
        {
            try
            {
                if (path.Contains('%'))
                {
                    expandedPath = Environment.ExpandEnvironmentVariables(path);
                    return true;
                }
            } catch (Exception ex) { Debug.WriteLine(ex.Message); }
            expandedPath = path;
            return false;
        }

        #endregion

        #region Exist Helpers
        /// <summary>
        /// Check if a file exists on the file system.
        /// </summary>
        /// <param name="filePath">The path to a file.</param>
        /// <param name="expandPath">Should the <paramref name="filePath"/> be expanded before checking?</param>
        /// <returns><c>true</c> if the file exists; <c>false</c> otherwise.</returns>
        public static bool DoesFileExist(string filePath, bool expandPath = true)
        {
            if (expandPath)
            {
                filePath = ExpandPath(filePath);
                return File.Exists(filePath);
            }

            return File.Exists(filePath);
        }

        /// <summary>
        /// Check if a folder exists on the file system.
        /// </summary>
        /// <param name="folderPath">The path to a folder.</param>
        /// <param name="expandPath">Should the <paramref name="folderPath"/> be expanded before checking?</param>
        /// <returns><c>true</c> if the folder exists; <c>false</c> otherwise.</returns>
        public static bool DoesFolderExist(string folderPath, bool expandPath = true)
        {
            if (expandPath)
            {
                folderPath = ExpandPath(folderPath);
                return Directory.Exists(folderPath);
            }
            return Directory.Exists(folderPath);
        }
        #endregion
    }
}
