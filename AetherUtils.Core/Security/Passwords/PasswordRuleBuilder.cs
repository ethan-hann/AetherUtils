namespace AetherUtils.Core.Security.Passwords;

/// <summary>
/// Internal builder class for a <see cref="PasswordRule"/>.
/// </summary>
internal sealed class PasswordRuleBuilder : IPasswordRuleBuilder
{
    private readonly PasswordRule _rule;

    internal PasswordRuleBuilder() => _rule = new PasswordRule(false);
    
    public IPasswordRuleBuilder AllowWhitespace()
    {
        _rule.AllowWhiteSpace();
        return this;
    }

    public IPasswordRuleBuilder AllowNumbers()
    {
        _rule.AllowNumbers();
        return this;
    }
    
    public IPasswordRuleBuilder MinimumLength(int length)
    {
        _rule.MinimumLength(length);
        return this;
    }

    public IPasswordRuleBuilder MinimumNumberCount(int count)
    {
        _rule.MinimumNumberCount(count);
        return this;
    }

    public IPasswordRuleBuilder MinimumSpecialCount(int count)
    {
        _rule.MinimumSpecialCount(count);
        return this;
    }

    public IPasswordRuleBuilder AllowSpecials()
    {
        _rule.AllowSpecials();
        return this;
    }

    public IPasswordRuleBuilder Expires(DateTime expires)
    {
        _rule.Expires(expires);
        return this;
    }

    public PasswordRule Build()
    {
        _rule.FinishedBuilding();
        return _rule;
    }
}