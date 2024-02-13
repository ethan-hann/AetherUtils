namespace AetherUtils.Core.Exceptions;

/// <summary>
/// Custom exception class for password rule validation.
/// </summary>
public class PasswordRuleException(string message) : Exception(message);