using AetherUtils.Core.Interfaces;

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
/// Builder interface for creating a new <see cref="PasswordRule"/>.
/// </summary>
public interface IPasswordRuleBuilder : IFluentInterface
{
    /// <summary>
    /// Allows a password to contain whitespace.
    /// </summary>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowWhitespace();
    
    /// <summary>
    /// Allows a password to contain special characters.
    /// </summary>
    /// <param name="specialList">An optional array of characters to allow as specials for the password.
    /// If not provided, the default list of specials will be used:<br/>
    /// <c>!&quot;#$%&amp;&apos;()*+,-./:;&lt;=&gt;?@[\]^_`{|}~</c>
    /// </param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowSpecials(char[]? specialList = null);

    /// <summary>
    /// Allows a password to contain numbers.
    /// </summary>
    /// <param name="numberList">An optional array of integers (supplied as characters) to allow as numbers in the password.
    /// If not provided, the default list of numbers will be used:<br/>
    /// <c>1234567890</c></param>
    /// <returns>The builder instance.</returns>
    IPasswordRuleBuilder AllowNumbers(char[]? numberList = null);

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
    /// Allow this password rule to be built based on the supplied template expression.
    /// </summary>
    /// <param name="template">The template to build the rule from.</param>
    /// <returns>The built <see cref="PasswordRule"/>.</returns>
    PasswordRule BuildFromTemplate(string template);

    /// <summary>
    /// Build and compile the password rule.
    /// </summary>
    /// <returns>The built <see cref="PasswordRule"/>.</returns>
    PasswordRule Build();
}