namespace AetherUtils.Core.Exceptions;

/// <summary>
/// Custom exception class for password rule validation.
/// </summary>
public class PasswordRuleException : Exception
{
    public PasswordRuleException() { }
    
    public PasswordRuleException(string message) : base(message) {}

    public PasswordRuleException(string message, Exception? inner) : base(message, inner) {}
}