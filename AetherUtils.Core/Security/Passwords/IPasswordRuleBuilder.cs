using AetherUtils.Core.Interfaces;

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
/// Builder interface for creating a new <see cref="PasswordRule"/>.
/// </summary>
public interface IPasswordRuleBuilder : IFluentInterface
{
    /// <summary>
    /// Allows the password to contain whitespace.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowWhitespace();
    
    /// <summary>
    /// Allows the password to contain special characters.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowSpecials();

    /// <summary>
    /// Allows the password to contain numbers.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowNumbers();

    /// <summary>
    /// Set the minimum length a password should be.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumLength(int length);
    
    /// <summary>
    /// Set the minimum count of numbers a password should contain.
    /// </summary>
    /// <param name="count">The minimum number of digits allowed.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumNumberCount(int count);

    /// <summary>
    /// Set the minimum count of special characters a password should contain.
    /// </summary>
    /// <param name="count">The minimum number of special characters allowed.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumSpecialCount(int count);

    /// <summary>
    /// Set the date that a password should expire at.
    /// </summary>
    /// <param name="expires">The expiration expires of passwords validated against the rule.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder Expires(DateTime expires);

    /// <summary>
    /// Build and compile the password rule.
    /// </summary>
    /// <returns>The built <see cref="PasswordRule"/>.</returns>
    PasswordRule Build();
}