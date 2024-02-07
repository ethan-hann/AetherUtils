namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Represents an interface for encryption classes to implement. Provides generic methods for encryption and decryption of data.
    /// <para><typeparamref name="T"/>: The input type for encryption.<br/>
    /// <typeparamref name="U"/>: The output type after decryption occurs.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The input type for encryption.</typeparam>
    /// <typeparam name="U">The output type after decryption.</typeparam>
    internal interface IEncryptService<T, U>
    {
        /// <summary>
        /// Encrypt the <typeparamref name="T"/>  <paramref name="input"/> using the specified passphrase.
        /// </summary>
        /// <param name="input">The data to encrypt.</param>
        /// <param name="passphrase">A passphrase used to derive the encryption key.</param>
        /// <returns>The <typeparamref name="U"/> encrypted data.</returns>
        public Task<U> EncryptAsync(T input, string passphrase);

        /// <summary>
        /// Decrypt the <typeparamref name="U"/> <paramref name="input"/> using the specified passphrase.
        /// </summary>
        /// <param name="input">The data to decrypt.</param>
        /// <param name="passphrase">The passphrase used to derive the decryption key.</param>
        /// <returns>The <typeparamref name="T"/> decrypted data.</returns>
        public Task<T> DecryptAsync(U input, string passphrase);
    }
}
