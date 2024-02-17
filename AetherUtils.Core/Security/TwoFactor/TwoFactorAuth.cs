using System.Security.Cryptography;
using System.Text;
using AetherUtils.Core.Exceptions;
using AetherUtils.Core.Extensions;
using QRCoder;

namespace AetherUtils.Core.Security.TwoFactor;

/// <summary>
/// Provides methods to generate and validate two-factor authentication codes.
/// </summary>
/// <param name="hashType">The <see cref="TwoFactor.HashType"/>for the generated codes.</param>
public class TwoFactorAuth(HashType hashType)
{
    private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private TimeSpan DefaultClockDriftTolerance { get; } = TimeSpan.FromMinutes(5);

    private HashType HashType { get; } = hashType;

    public TwoFactorAuth() : this(HashType.Sha1) { }

    /// <summary>
    /// Generate the setup information needed for a user to scan with their authenticator app.
    /// </summary>
    /// <param name="user">The <see cref="TwoFactorUser"/> containing the setup information, as a reference.</param>
    /// <param name="secretKey">The secret key used to generate the setup information.</param>
    /// <returns>SetupCode object</returns>
    public void GenerateSetupInformation(ref TwoFactorUser user, byte[] secretKey)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentException.ThrowIfNullOrEmpty(user.AccountTitle, nameof(user));
        ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

        var secretAsBase32Bytes = ConvertSecretToBytes(Encoder.Base32Encode(secretKey), true);

        user.AccountTitle = Uri.EscapeDataString(user.AccountTitle).RemoveWhitespace();

        var encodedSecretKey = Encoder.Base32Encode(secretAsBase32Bytes);

        var provisionUrl = string.IsNullOrWhiteSpace(user.Issuer)
            ? $"otpauth://totp/{user.AccountTitle}?secret={encodedSecretKey.Trim('=')}{(HashType == HashType.Sha1 ? "" : $"&algorithm={HashType}")}"
            //  https://github.com/google/google-authenticator/wiki/Conflicting-Accounts
            // Added additional prefix to account otpauth://totp/Company:joe_example@gmail.com
            // for backwards compatibility
            : $"otpauth://totp/{UrlEncode(user.Issuer)}:{user.AccountTitle}?secret={encodedSecretKey.Trim('=')}&issuer={UrlEncode(user.Issuer)}{(HashType == HashType.Sha1 ? "" : $"&algorithm={HashType}")}";

        var sc = new SetupCode(encodedSecretKey.Trim('='), GenerateQrCodeUrl(3, provisionUrl));
        user.SetupInformation = sc;
    }

    private static string GenerateQrCodeUrl(int qrPixelsPerModule, string provisionUrl)
    {
        string? qrCodeUrl;
        try
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(provisionUrl, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(qrPixelsPerModule);
            qrCodeUrl = $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
        }
        catch (System.Runtime.InteropServices.ExternalException e)
        {
            if (e.Message.Contains("GDI+") && qrPixelsPerModule > 10)
            {
                throw new QrException(
                    $"There was a problem generating a QR code. The value of {nameof(qrPixelsPerModule)}" +
                    " should be set to a value of 10 or less for optimal results.",
                    e);
            }

            throw;
        }

        return qrCodeUrl;
    }
    
    private static string UrlEncode(string value)
    {
        return Uri.EscapeDataString(value);
    }

    /// <summary>
    /// This method is generally called via <see cref="GetCurrentPin(string,bool)"/>
    /// </summary>
    /// <param name="accountSecretKey">The account secret key as a string</param>
    /// <param name="counter">The number of 30-second (by default) intervals since the unix epoch</param>
    /// <param name="digits">The desired length of the returned PIN</param>
    /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
    /// <returns>A 'PIN' that is valid for the specified time interval</returns>
    public string GeneratePinAtInterval(string accountSecretKey, long counter, int digits = 6, bool secretIsBase32 = false) =>
        GeneratePinAtInterval(ConvertSecretToBytes(accountSecretKey, secretIsBase32), counter, digits);

    /// <summary>
    /// This method is generally called via <see cref="GetCurrentPin(string,bool)" />/>
    /// </summary>
    /// <param name="accountSecretKey">The account secret key as a byte array</param>
    /// <param name="counter">The number of 30-second (by default) intervals since the unix epoch</param>
    /// <param name="digits">The desired length of the returned PIN</param>
    /// <returns>A 'PIN' that is valid for the specified time interval</returns>
    public string GeneratePinAtInterval(byte[] accountSecretKey, long counter, int digits = 6) =>
        GenerateHashedCode(accountSecretKey, counter, digits);

    private string GenerateHashedCode(byte[] key, long iterationNumber, int digits = 6)
    {
        var counter = BitConverter.GetBytes(iterationNumber);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(counter);

        HMAC hmac = HashType switch
        {
            HashType.Sha256 => new HMACSHA256(key),
            HashType.Sha512 => new HMACSHA512(key),
            _ => new HMACSHA1(key)
        };

        var hash = hmac.ComputeHash(counter);
        var offset = hash[^1] & 0xf;

        // Convert the 4 bytes into an integer, ignoring the sign.
        var binary =
            ((hash[offset] & 0x7f) << 24)
            | (hash[offset + 1] << 16)
            | (hash[offset + 2] << 8)
            | hash[offset + 3];

        var password = binary % (int) Math.Pow(10, digits);
        return password.ToString(new string('0', digits));
    }

    private static long GetCurrentCounter() => GetCurrentCounter(DateTime.UtcNow, Epoch, 30);

    private static long GetCurrentCounter(DateTime now, DateTime epoch, int timeStep) =>
        (long) (now - epoch).TotalSeconds / timeStep;
    
    /// <summary>
    /// Given a PIN from a client, check if it is valid at the current time.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="twoFactorCodeFromClient">The PIN from the client</param>
    /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
    /// <returns>True if PIN is currently valid</returns>
    public bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient, bool secretIsBase32 = false) =>
        ValidateTwoFactorPin(accountSecretKey, twoFactorCodeFromClient, DefaultClockDriftTolerance, secretIsBase32);

    /// <summary>
    /// Given a PIN from a client, check if it is valid at the current time.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="twoFactorCodeFromClient">The PIN from the client</param>
    /// <param name="timeTolerance">The time window within which to check to allow for clock drift between devices.</param>
    /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
    /// <returns>True if PIN is currently valid</returns>
    public bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient, TimeSpan timeTolerance, bool secretIsBase32 = false) =>
        ValidateTwoFactorPin(ConvertSecretToBytes(accountSecretKey, secretIsBase32), twoFactorCodeFromClient, timeTolerance);

    /// <summary>
    /// Given a PIN from a client, check if it is valid at the current time.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="twoFactorCodeFromClient">The PIN from the client</param>
    /// <returns>True if PIN is currently valid</returns>
    public bool ValidateTwoFactorPin(byte[] accountSecretKey, string twoFactorCodeFromClient) =>
        ValidateTwoFactorPin(accountSecretKey, twoFactorCodeFromClient, DefaultClockDriftTolerance);

    /// <summary>
    /// Given a PIN from a client, check if it is valid at the current time.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="twoFactorCodeFromClient">The PIN from the client</param>
    /// <param name="timeTolerance">The time window within which to check to allow for clock drift between devices.</param>
    /// <returns>True if PIN is currently valid</returns>
    public bool ValidateTwoFactorPin(byte[] accountSecretKey, string twoFactorCodeFromClient, TimeSpan timeTolerance) => 
        GetCurrentPins(accountSecretKey, timeTolerance).Any(c => c == twoFactorCodeFromClient);

    /// <summary>
    /// Given a PIN from a client, check if it is valid at the current time.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="twoFactorCodeFromClient">The PIN from the client</param>
    /// <param name="iterationOffset">The counter window within which to check to allow for clock drift between devices.</param>
    /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
    /// <returns>True if PIN is currently valid</returns>
    public bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient, int iterationOffset, bool secretIsBase32 = false) => 
        ValidateTwoFactorPin(ConvertSecretToBytes(accountSecretKey, secretIsBase32), twoFactorCodeFromClient, iterationOffset);
    
    /// <summary>
    /// Given a PIN from a client, check if it is valid at the current time.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="twoFactorCodeFromClient">The PIN from the client</param>
    /// <param name="iterationOffset">The counter window within which to check to allow for clock drift between devices.</param>
    /// <returns>True if PIN is currently valid</returns>
    public bool ValidateTwoFactorPin(byte[] accountSecretKey, string twoFactorCodeFromClient, int iterationOffset) => 
        GetCurrentPins(accountSecretKey, iterationOffset).Any(c => c == twoFactorCodeFromClient);
    
    /// <summary>
    /// Get the PIN for current time; the same code that a 2FA app would generate for the current time.
    /// Do not validate directly against this as clock drift may cause a different PIN to be generated than one you did a second ago.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
    /// <returns>A 6-digit PIN</returns>
    public string GetCurrentPin(string accountSecretKey, bool secretIsBase32 = false) =>
        GeneratePinAtInterval(accountSecretKey, GetCurrentCounter(), secretIsBase32: secretIsBase32);

    /// <summary>
    /// Get the PIN for current time; the same code that a 2FA app would generate for the current time.
    /// Do not validate directly against this as clock drift may cause a different PIN to be generated than one you did a second ago.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="now">The time you wish to generate the pin for</param>
    /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
    /// <returns>A 6-digit PIN</returns>
    public string GetCurrentPin(string accountSecretKey, DateTime now, bool secretIsBase32 = false) =>
        GeneratePinAtInterval(accountSecretKey, GetCurrentCounter(now, Epoch, 30), secretIsBase32: secretIsBase32);

    /// <summary>
    /// Get the PIN for current time; the same code that a 2FA app would generate for the current time.
    /// Do not validate directly against this as clock drift may cause a different PIN to be generated.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <returns>A 6-digit PIN</returns>
    public string GetCurrentPin(byte[] accountSecretKey) =>
        GeneratePinAtInterval(accountSecretKey, GetCurrentCounter());

    /// <summary>
    /// Get the PIN for current time; the same code that a 2FA app would generate for the current time.
    /// Do not validate directly against this as clock drift may cause a different PIN to be generated.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="now">The time you wish to generate the pin for</param>
    /// <returns>A 6-digit PIN</returns>
    public string GetCurrentPin(byte[] accountSecretKey, DateTime now) =>
        GeneratePinAtInterval(accountSecretKey, GetCurrentCounter(now, Epoch, 30));

    /// <summary>
    /// Get all the PINs that would be valid within the time window allowed for by the default clock drift.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
    /// <returns></returns>
    public string[] GetCurrentPins(string accountSecretKey, bool secretIsBase32 = false) =>
        GetCurrentPins(accountSecretKey, DefaultClockDriftTolerance, secretIsBase32);

    /// <summary>
    /// Get all the PINs that would be valid within the time window allowed for by the specified clock drift.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="timeTolerance">The clock drift size you want to generate PINs for</param>
    /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
    /// <returns></returns>
    public string[] GetCurrentPins(string accountSecretKey, TimeSpan timeTolerance, bool secretIsBase32 = false) =>
        GetCurrentPins(ConvertSecretToBytes(accountSecretKey, secretIsBase32), timeTolerance);

    /// <summary>
    /// Get all the PINs that would be valid within the time window allowed for by the default clock drift.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <returns></returns>
    public string[] GetCurrentPins(byte[] accountSecretKey) =>
        GetCurrentPins(accountSecretKey, DefaultClockDriftTolerance);

    /// <summary>
    /// Get all the PINs that would be valid within the time window allowed for by the specified clock drift.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="timeTolerance">The clock drift size you want to generate PINs for</param>
    /// <returns></returns>
    public string[] GetCurrentPins(byte[] accountSecretKey, TimeSpan timeTolerance)
    {
        var iterationOffset = 0;

        if (timeTolerance.TotalSeconds >= 30)
            iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30.00);

        return GetCurrentPins(accountSecretKey, iterationOffset);
    }
    
    /// <summary>
    /// Get all the PINs that would be valid within the time window allowed for by the specified clock drift.
    /// </summary>
    /// <param name="accountSecretKey">Account Secret Key</param>
    /// <param name="iterationOffset">The counter drift size you want to generate PINs for</param>
    /// <returns></returns>
    public string[] GetCurrentPins(byte[] accountSecretKey, int iterationOffset)
    {
        var codes = new List<string>();
        var iterationCounter = GetCurrentCounter();
        
        var iterationStart = iterationCounter - iterationOffset;
        var iterationEnd = iterationCounter + iterationOffset;

        for (var counter = iterationStart; counter <= iterationEnd; counter++)
        {
            codes.Add(GeneratePinAtInterval(accountSecretKey, counter));
        }

        return codes.ToArray();
    }

    private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32) =>
        secretIsBase32 ? Encoder.ToBytes(secret) : Encoding.UTF8.GetBytes(secret);
}