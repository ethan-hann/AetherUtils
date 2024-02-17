namespace AetherUtils.Core.Security.Encryption
{
    /// <summary>
    /// Represents an interface for encryption classes to implement.
    /// Provides generic methods for encryption and decryption of data.
    /// </summary>
    /// <typeparam name="TInput">The input type for encryption.</typeparam>
    /// <typeparam name="TOutput">The output type after decryption.</typeparam>
    internal interface IEncryptService<TInput, TOutput>
    {
        /// <summary>
        /// Encrypt the <typeparamref name="TInput"/>  <paramref name="input"/> using the specified passphrase.
        /// </summary>
        /// <param name="input">The data to encrypt.</param>
        /// <param name="passphrase">A passphrase used to derive the encryption key.</param>
        /// <returns>The <typeparamref name="TOutput"/> encrypted data.</returns>
        public Task<TOutput> EncryptAsync(TInput input, string passphrase);

        /// <summary>
        /// Decrypt the <typeparamref name="TOutput"/> <paramref name="input"/> using the specified passphrase.
        /// </summary>
        /// <param name="input">The data to decrypt.</param>
        /// <param name="passphrase">The passphrase used to derive the decryption key.</param>
        /// <returns>The <typeparamref name="TInput"/> decrypted data.</returns>
        public Task<TInput> DecryptAsync(TOutput input, string passphrase);
    }
}
