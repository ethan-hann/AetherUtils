namespace AetherUtils.Core.Security.TwoFactor;

/// <summary>
/// Represents a setup code needed by the user to setup two factor authentication.
/// </summary>
/// <param name="manualEntryKey">The secret key that can be manually entered in a 2FA app.</param>
/// <param name="qrCodeSetupImageUrl">The base64 string representing the QR code.</param>
public sealed class SetupCode(string manualEntryKey, string qrCodeSetupImageUrl)
{
    /// <summary>
    /// The secret key that can be manually entered in a 2FA app.
    /// </summary>
    public string ManualEntryKey { get; } = manualEntryKey;
    
    /// <summary>
    /// The base64 string representing the QR code.
    /// </summary>
    public string QrCodeSetupImageUrl { get; } = qrCodeSetupImageUrl;
}