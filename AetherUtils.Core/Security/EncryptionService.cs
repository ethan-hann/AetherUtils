using AetherUtils.Core.Extensions;
using AetherUtils.Core.Files;
using System.Security.Cryptography;
using System.Text;

namespace AetherUtils.Core.Security
{
    //Implemented based on article found here: https://jonathancrozier.com/blog/how-to-implement-symmetric-encryption-in-a-dot-net-app-using-aes
    
    /// <summary>
    /// Provides a service for encryption/decryption of <see cref="string"/>s and <see cref="object"/>s.
    /// <para>This class encrypts objects and strings using AES-256 encryption algorithms.</para>
    /// </summary>
    public class EncryptionService
    {
        /// <summary>
        /// The key size in bytes.
        /// </summary>
        private const int KEY_SIZE = 32; //32 bytes = 256-bit

        /// <summary>
        /// The prefix to add to the beginning of an encrypted string.
        /// </summary>
        private const string ENCRYPTED_VALUE_PREFIX = "AU:";

        /// <summary>
        /// Determines if the input string is encrypted.
        /// </summary>
        /// <param name="input">The string to check</param>
        /// <returns><c>true</c> if the string is encrypted; <c>false</c> otherwise.</returns>
        public static bool IsEncrypted(string input) =>
            input.StartsWith(ENCRYPTED_VALUE_PREFIX, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Encrypt the object <typeparamref name="T"/> to an encrypted XML string.
        /// <para><typeparamref name="T"/> must support XML serialization.</para>
        /// </summary>
        /// <param name="input">The object to encrypt.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The encrypted object's string or <c>null</c> if encryption failed.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="input"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If <paramref name="input"/> does not support XML serialization.</exception>
        public string? EncryptObject<T>(T input, byte[] key) where T : class
        {
            ArgumentNullException.ThrowIfNull(input);

            if (!input.CanSerialize())
                throw new InvalidOperationException($"{input.GetType()} does not support XML serialization.");

            string? objectString;

            if (input is string inputString) //If the input is a string, just pass it along to the EncryptString method.
                return EncryptString(inputString, key);
            else
                objectString = input.Serialize(); //Otherwise, serialize the object to an XML string first.

            if (objectString is string obj)
                return EncryptString(obj, key); //After the object has been serialized, we can encrypt it.

            return null;
        }

        /// <summary>
        /// Decrypt an encrypted XML string containing an object of type <typeparamref name="T"/>.
        /// <para><typeparamref name="T"/> must support serialization.</para>
        /// </summary>
        /// <param name="input">The encrypted XML string.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The decrypted <typeparamref name="T"/> object, or <c>null</c> if decryption failed.</returns>
        /// <exception cref="ArgumentException">If <paramref name="input"/> is <c>null</c> or empty.</exception>
        public T? DecryptObject<T>(string input, byte[] key) where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(input);

            string decrypted = DecryptString(input, key); //Decrypt the encrypted XML string and then deserialize it back to a .NET object.

            return decrypted.Deserialize<T>();
            //return JsonConvert.DeserializeObject<T>(decrypted);
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="input">The text to encrypt</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The encrypted text</returns>
        public string EncryptString(string input, byte[] key)
        {
            if (string.IsNullOrWhiteSpace(input) || IsEncrypted(input))
                // There is no need to encrypt null/empty or already encrypted text.
                return input;

            // Create a new random vector.
            byte[] vector = GenerateInitializationVector();

            // Encrypt the text.
            string encryptedText = Convert.ToBase64String(Encrypt(input, key, vector));

            // Format and return the encrypted data.
            return ENCRYPTED_VALUE_PREFIX + Convert.ToBase64String(vector) + ";" + encryptedText;
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="input">The text to decrypt</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The decrypted text</returns>
        public string DecryptString(string input, byte[] key)
        {
            if (string.IsNullOrWhiteSpace(input) || !IsEncrypted(input))
                // There is no need to decrypt null/empty or unencrypted text.
                return input;

            // Parse the vector from the encrypted data.
            byte[] vector = Convert.FromBase64String(input.Split(';')[0].Split(':')[1]);

            // Decrypt and return the plain text.
            return Decrypt(Convert.FromBase64String(input.Split(';')[1]), key, vector);
        }

        /// <summary>
        /// Get a cryptographically strong random key that can be used for encryption/decryption.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetRandomKey()
        {
            var bytes = new byte[KEY_SIZE];
            RandomNumberGenerator.Create().GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Save the encrypted string contents to a new file.
        /// </summary>
        /// <param name="fileName">The full path to the file to save.</param>
        /// <param name="encryptedContents">The encrypted contents to save.</param>
        /// <exception cref="ArgumentException">If the <paramref name="encryptedContents"/> are not encrypted.</exception>
        public static void SaveToFile(string fileName, string encryptedContents)
        {
            if (!IsEncrypted(encryptedContents))
                throw new ArgumentException("Contents are not encrypted!", nameof(encryptedContents));

            fileName = FileHelper.ExpandPath(fileName);
            FileHelper.SaveFileAsync(fileName, encryptedContents, Encoding.UTF8, false);
        }

        /// <summary>
        /// Load a file containing encrypted data.
        /// </summary>
        /// <param name="fileName">The full path of the file to load.</param>
        /// <returns>An encrypted string representing the file's contents.</returns>
        public static string LoadFromFile(string fileName) => FileHelper.OpenFile(fileName);

        /// <summary>
        /// Decrypts the specified byte array to plain text.
        /// </summary>
        /// <param name="encryptedBytes">The encrypted byte array</param>
        /// <param name="key">The encryption key</param>
        /// <param name="vector">The initialization vector</param>
        /// <returns>The decrypted text as a string</returns>
        private static string Decrypt(byte[] encryptedBytes, byte[] key, byte[] vector)
        {
            using var aesAlgo = Aes.Create();
            aesAlgo.Padding = PaddingMode.PKCS7;
            aesAlgo.Mode = CipherMode.CBC;
            aesAlgo.Key = key;
            aesAlgo.IV = vector;

            byte[] data = [];

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aesAlgo.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(encryptedBytes, 0, encryptedBytes.Length);
                }
                data = ms.ToArray();
            }
            return Encoding.UTF8.GetString(data);
            //string str = string.Empty;

            //using var aesAlgo = Aes.Create();
            //aesAlgo.Padding = PaddingMode.ISO10126;

            //using var decryptor = aesAlgo.CreateDecryptor(key, vector);
            //using var memoryStream = new MemoryStream(encryptedBytes);
            //using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            //{
            //    using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
            //    {
            //        str = streamReader.ReadToEnd();
            //    }
            //}

            //return str;
        }

        /// <summary>
        /// Encrypts the specified text and returns an encrypted byte array.
        /// </summary>
        /// <param name="input">The text to encrypt</param>
        /// <param name="key">The encryption key</param>
        /// <param name="vector">The initialization vector</param>
        /// <returns>The encrypted text as a byte array</returns>
        private static byte[] Encrypt(string input, byte[] key, byte[] vector)
        {
            using var aesAlgo = Aes.Create();
            aesAlgo.Padding = PaddingMode.PKCS7;
            aesAlgo.Mode = CipherMode.CBC;
            aesAlgo.Key = key;
            aesAlgo.IV = vector;

            byte[] data = Encoding.UTF8.GetBytes(input);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aesAlgo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }
                return ms.ToArray();
            }

            //using var encryptor = aesAlgo.CreateEncryptor(key, vector);
            //using var memoryStream = new MemoryStream();
            //using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            //{
            //    using (var streamWriter = new StreamWriter(cryptoStream, Encoding.UTF8))
            //    {
            //        streamWriter.Write(input);
            //        streamWriter.Flush();
            //    }
            //}
            
            //return memoryStream.ToArray();
        }

        /// <summary>
        /// Generates a random initialization vector.
        /// </summary>
        /// <returns>The initialization vector as a byte array</returns>
        private byte[] GenerateInitializationVector()
        {
            using (var aesAlgo = Aes.Create())
            {
                aesAlgo.GenerateIV();
                return aesAlgo.IV;
            }
        }
    }
}
