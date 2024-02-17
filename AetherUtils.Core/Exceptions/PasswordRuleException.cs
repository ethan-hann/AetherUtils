namespace AetherUtils.Core.Exceptions;

/// <summary>
/// Custom exception class for password rule validation.
/// </summary>
public sealed class PasswordRuleException(string message) : Exception(message);