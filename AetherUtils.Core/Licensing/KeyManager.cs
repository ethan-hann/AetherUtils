﻿using AetherUtils.Core.Files;
using Newtonsoft.Json;
using Standard.Licensing.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Licensing
{
    /// <summary>
    /// Handles loading and saving private and public keys to file system.
    /// </summary>
    public static class KeyManager
    {
        /// <summary>
        /// Save the key pair to a file.
        /// </summary>
        /// <param name="keys">The pair of keys (private and public). Private key should be first.</param>
        /// <param name="basePath">The base folder to save the keys in.</param>
        /// <param name="fileName">The name of the file to save: default is <c>keyPair.keys</c></param>
        public static void SaveKeys(KeyPair keys, string basePath, string fileName = "keyPair.keys")
        {
            var json = JsonConvert.SerializeObject(keys);
            try
            {
                string fullPath = Path.Combine(basePath, fileName);
                fullPath = FileHelper.ExpandPath(fullPath);
                FileHelper.CreateDirectories(fullPath);

                File.WriteAllText(fullPath, json);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }

        /// <summary>
        /// Save only the public key to a file.
        /// </summary>
        /// <param name="keys">The pair of keys (private and public). Private key should be first.</param>
        /// <param name="basePath">The base folder to save the public key in.</param>
        /// <param name="fileName">The name of the file to save: default is <c>public.key</c></param>
        public static void SavePublicKey(KeyPair keys, string basePath, string fileName = "public.key")
        {
            string publicKey = keys.PublicKey;
            var json = JsonConvert.SerializeObject(publicKey);
            try
            {
                string fullPath = Path.Combine(basePath, fileName);
                fullPath = FileHelper.ExpandPath(fullPath);
                FileHelper.CreateDirectories(fullPath);

                File.WriteAllText(fullPath, json);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }

        /// <summary>
        /// Save only the public key to a file.
        /// </summary>
        /// <param name="publicKey">The public key to save.</param>
        /// <param name="basePath">The base folder to save the public key in.</param>
        /// <param name="fileName">The name of the file to save: default is <c>public.key</c></param>
        public static void SavePublicKey(string publicKey, string basePath, string fileName = "public.key")
        {
            var json = JsonConvert.SerializeObject(publicKey);
            try
            {
                string fullPath = Path.Combine(basePath, fileName);
                fullPath = FileHelper.ExpandPath(fullPath);
                FileHelper.CreateDirectories(fullPath);

                File.WriteAllText(fullPath, json);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }

        /// <summary>
        /// Load a key pair from a file.
        /// </summary>
        /// <param name="basePath">The base folder the keys are located in.</param>
        /// <param name="fileName">The name of the file to load: default is <c>keyPair.keys</c></param>
        /// <returns>A <see cref="KeyPair"/> representing the key values.</returns>
        public static KeyPair? LoadKeys(string basePath, string fileName = "keyPair.keys")
        {
            string fullPath = Path.Combine(basePath, fileName);
            fullPath = FileHelper.ExpandPath(fullPath);
            try
            {
                string text = File.ReadAllText(fullPath);
                return JsonConvert.DeserializeObject<KeyPair>(text);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null;
        }

        /// <summary>
        /// Load a key pair from a file.
        /// </summary>
        /// <param name="fullPath">The full, absolute path to a key file.</param>
        /// <returns>A <see cref="KeyPair"/> representing the key values.</returns>
        public static KeyPair? LoadKeys(string fullPath)
        {
            fullPath = FileHelper.ExpandPath(fullPath);
            try
            {
                string text = File.ReadAllText(fullPath);
                return JsonConvert.DeserializeObject<KeyPair>(text);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return null;
        }

        /// <summary>
        /// Get a value indicating whether the keyPair.keys file exists at the specified base path.
        /// </summary>
        /// <param name="basePath">The base folder the keys are located in.</param>
        /// <param name="fileName">The name of the file to check: default is <c>keyPair.keys</c></param>
        /// <returns><c>true</c> if the file was found; <c>false</c>, otherwise.</returns>
        public static bool DoesKeyFileExist(string basePath, string fileName = "keyPair.keys")
        {
            string fullPath = Path.Combine(basePath, fileName);
            return FileHelper.DoesFileExist(fullPath);
        }
    }
}