namespace AetherUtils.Core.Security.Passwords;

internal class PasswordRuleBuilder : IPasswordRuleBuilder
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

    public IPasswordRuleBuilder MinimumLetterCount(int count)
    {
        _rule.MinimumLetterCount(count);
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

    public IPasswordRuleBuilder Expires(DateTime date)
    {
        _rule.Expires(date);
        return this;
    }

    public PasswordRule Build()
    {
        _rule.FinishedBuilding();
        return _rule;
    }
}