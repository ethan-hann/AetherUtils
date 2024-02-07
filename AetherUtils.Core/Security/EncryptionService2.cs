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
    public class EncryptionService2
    {
        /// <summary>
        /// The key size in bytes.
        /// </summary>
        private const int KEY_SIZE = 32; //32 bytes = 256-bit

        public static byte[] EncryptString(string input, byte[] key)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.GenerateIV();

                        byte[] iv = aes.IV;
                        ms.Write(iv, 0, iv.Length);

                        using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (StreamWriter encryptWriter = new(cs))
                            {
                                encryptWriter.Write(input);
                                encryptWriter.Flush();
                            }
                        }
                    }
                    return ms.ToArray();
                }
            } catch (Exception ex) { Console.WriteLine(ex); return []; }
        }

        public static async Task<string> DecryptString(byte[] encryptedBytes, byte[] key) 
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(encryptedBytes))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[aes.IV.Length];
                        int numBytesToRead = aes.IV.Length;
                        int numOfBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = ms.Read(iv, numOfBytesRead, numBytesToRead);
                            if (n == 0) break;

                            numOfBytesRead += n;
                            numBytesToRead -= n;
                        }

                        aes.Key = key;
                        aes.IV = iv;

                        using (CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader decryptReader = new(cs))
                            {
                                string message = await decryptReader.ReadToEndAsync();
                                return message;
                            }
                        }
                    }
                }
            } catch (Exception ex) { Console.WriteLine(ex); return string.Empty; }
        }

        public static bool EncryptFile(string fileContent, string filePath, byte[] key)
        {
            try
            {
                using (FileStream fileStream = new(filePath, FileMode.OpenOrCreate))
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;

                        byte[] iv = aes.IV;
                        fileStream.Write(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new(
                            fileStream,
                            aes.CreateEncryptor(),
                            CryptoStreamMode.Write))
                        {
                            // By default, the StreamWriter uses UTF-8 encoding.
                            // To change the text encoding, pass the desired encoding as the second parameter.
                            // For example, new StreamWriter(cryptoStream, Encoding.Unicode).
                            using (StreamWriter encryptWriter = new(cryptoStream))
                            {
                                encryptWriter.WriteLine(fileContent);
                            }
                        }
                    }
                }

                Console.WriteLine("The file was encrypted.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
                return false;
            }
        }

        //public static bool EncryptFile(string fileContent, string filePath, byte[] key)
        //{
        //    try
        //    {
        //        using FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
        //        using Aes aes = Aes.Create();
        //        aes.Key = key;
        //        aes.GenerateIV();

        //        fs.Write(aes.IV, 0, aes.IV.Length);

        //        using CryptoStream cs = new(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
        //        using StreamWriter encryptWriter = new(cs);
        //        encryptWriter.Write(fileContent);
                
        //        return true;
        //    } catch (Exception ex) { Console.WriteLine(ex); return false; }
        //}

        public static string DecryptFile(string filePath, byte[] key)
        {
            try
            {
                using (FileStream fileStream = new(filePath, FileMode.Open))
                {
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[aes.IV.Length];
                        int numBytesToRead = aes.IV.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                            if (n == 0) break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }

                        using (CryptoStream cryptoStream = new(
                           fileStream,
                           aes.CreateDecryptor(key, iv),
                           CryptoStreamMode.Read))
                        {
                            // By default, the StreamReader uses UTF-8 encoding.
                            // To change the text encoding, pass the desired encoding as the second parameter.
                            // For example, new StreamReader(cryptoStream, Encoding.Unicode).
                            using (StreamReader decryptReader = new(cryptoStream))
                            {
                                var message = decryptReader.ReadToEnd();
                                Console.WriteLine(message);
                                return message;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The decryption failed. {ex}");
                return string.Empty;
            }
        }

        //public static async Task<string> DecryptFile(string filePath, byte[] key)
        //{
        //    try
        //    {
        //        using (FileStream fs = new(filePath, FileMode.Open))
        //        {
        //            using (Aes aes = Aes.Create())
        //            {
        //                byte[] iv = new byte[aes.IV.Length];
        //                int numBytesToRead = aes.IV.Length;
        //                int numBytesRead = 0;
        //                while (numBytesToRead > 0)
        //                {
        //                    int n = fs.Read(iv, numBytesRead, numBytesToRead);
        //                    if (n == 0) break;

        //                    numBytesRead += n;
        //                    numBytesToRead -= n;
        //                }

        //                aes.Key = key;
        //                aes.IV = iv;

        //                using (CryptoStream cs = new(fs, aes.CreateDecryptor(), CryptoStreamMode.Read))
        //                {
        //                    using (StreamReader decryptReader = new(cs))
        //                    {
        //                        string message = await decryptReader.ReadToEndAsync();
        //                        return message;
        //                    }
        //                }
        //            }
        //        }
        //    } catch (Exception ex) { Console.WriteLine(ex); return string.Empty; }
        //}

        public static bool EncryptObjectToFile<T>(T input, string filePath, byte[] key) where T : class
        {
            ArgumentNullException.ThrowIfNull(input);

            if (!input.CanSerialize())
                throw new InvalidOperationException($"{input.GetType()} does not support XML serialization.");

            string? objectString;

            if (input is string inputString)
                return EncryptFile(inputString, filePath, key);
            else
                objectString = input.Serialize();

            if (objectString is string obj)
                return EncryptFile(obj, filePath, key);

            return false;
        }

        public static T? DecryptObjectFromFile<T>(string fileName, byte[] key) where T : class
        {
            ArgumentNullException.ThrowIfNull(fileName);

            var decryptedFile = DecryptFile(fileName, key);
            //string objString = decryptedFile;

            return decryptedFile.Deserialize<T>();
        }

        /// <summary>
        /// Encrypt the object <typeparamref name="T"/> to an encrypted XML string.
        /// <para><typeparamref name="T"/> must support XML serialization.</para>
        /// </summary>
        /// <param name="input">The object to encrypt.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The encrypted object's string or <c>null</c> if encryption failed.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="input"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If <paramref name="input"/> does not support XML serialization.</exception>
        public static byte[] EncryptObject<T>(T input, byte[] key) where T : class
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

            return [];
        }

        /// <summary>
        /// Decrypt an encrypted XML string containing an object of type <typeparamref name="T"/>.
        /// <para><typeparamref name="T"/> must support serialization.</para>
        /// </summary>
        /// <param name="input">The encrypted XML string.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The decrypted <typeparamref name="T"/> object, or <c>null</c> if decryption failed.</returns>
        /// <exception cref="ArgumentException">If <paramref name="input"/> is <c>null</c> or empty.</exception>
        public static T? DecryptObject<T>(byte[] input, byte[] key) where T : class
        {
            if (input.Length <= 0 || input == null) throw new ArgumentNullException(nameof(input));

            var decrypted = DecryptString(input, key); //Decrypt the encrypted XML string and then deserialize it back to a .NET object.

            return decrypted.Result.Deserialize<T>();
            //return JsonConvert.DeserializeObject<T>(decrypted);
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
