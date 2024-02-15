namespace AetherUtils.Core.Security.TwoFactor;

public sealed class SetupCode(string manualEntryKey, string qrCodeSetupImageUrl)
{
    public string ManualEntryKey { get; internal set; } = manualEntryKey;
    public string QrCodeSetupImageUrl { get; internal set; } = qrCodeSetupImageUrl;
}