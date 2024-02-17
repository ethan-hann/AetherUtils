namespace AetherUtils.Core.Security.TwoFactor;

/// <summary>
/// Represents a 2-factor authentication user.
/// </summary>
/// <param name="issuer">The issuer of the 2FA tokens.</param>
/// <param name="accountTitle">The account title as shown in the user's authenticator app.</param>
public sealed class TwoFactorUser(string issuer, string accountTitle)
{
    /// <summary>
    /// The issuer of the 2FA tokens. 
    /// </summary>
    public string Issuer { get; } = issuer;
    
    /// <summary>
    /// The account title as shown in the user's authenticator app.
    /// </summary>
    public string AccountTitle { get; internal set; } = accountTitle;
    
    /// <summary>
    /// The information needed for the user to set the account up in their authenticator app.
    /// </summary>
    public SetupCode? SetupInformation { get; internal set; }
}