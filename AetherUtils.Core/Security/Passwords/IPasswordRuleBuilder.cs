using AetherUtils.Core.Interfaces;

namespace AetherUtils.Core.Security.Passwords;

public interface IPasswordRuleBuilder : IFluentInterface
{
    /// <summary>
    /// Allows the password to contain whitespace.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowWhitespace();

    /// <summary>
    /// Allows the password to contain numbers.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowNumbers();

    /// <summary>
    /// Sets the minimum length a password can be.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumLength(int length);
    
    /// <summary>
    /// Sets the minimum number of letters allowed for the password.
    /// </summary>
    /// <param name="count">The minimum number of letters allowed.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumLetterCount(int count);
    
    /// <summary>
    /// Sets the minimum number of digits allowed for the password.
    /// </summary>
    /// <param name="count">The minimum number of digits allowed.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumNumberCount(int count);

    /// <summary>
    /// Sets the minimum number of special characters allowed for the password.
    /// </summary>
    /// <param name="count">The minimum number of special characters allowed.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder MinimumSpecialCount(int count);

    /// <summary>
    /// Allows the password to contain special characters.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowSpecials();

    /// <summary>
    /// Sets the date and time that the password should expire at.
    /// </summary>
    /// <param name="date">The date and time to expire the password.</param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder Expires(DateTime date);

    PasswordRule Build();
}