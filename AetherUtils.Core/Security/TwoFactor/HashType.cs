namespace AetherUtils.Core.Security.TwoFactor;

/// <summary>
/// Represents the various hash types valid for two factor authentication keys.
/// </summary>
public enum HashType
{
    /// <summary>
    /// Indicates the 2FA codes should use the SHA1 algorithm.
    /// </summary>
    Sha1,
    /// <summary>
    /// Indicates the 2FA codes should use the SHA256 algorithm.
    /// </summary>
    Sha256,
    /// <summary>
    /// Indicates the 2FA codes should use the SHA512 algorithm.
    /// </summary>
    Sha512
}