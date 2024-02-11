using System.Text;
using AetherUtils.Core.Exceptions;
using AetherUtils.Core.Files;
using AetherUtils.Core.Security.Passwords.Validation;

namespace AetherUtils.Core.Security.Passwords;

/// <summary>
/// 
/// </summary>
public sealed class PasswordRule
{
    private bool _isBuilding;
    private bool _isBuilt;
    
    private readonly StringBuilder _builder = new();

    internal PasswordRuleData RuleData = new();

    private readonly Json<PasswordRuleData> _serializer = new();

    public string PasswordRules { get; private set; } = string.Empty;
    
    internal PasswordRule(bool isBuilt)
    {
        _isBuilt = isBuilt;
        _isBuilding = !_isBuilt;
    }

    private PasswordRule(string json)
    {
        var p = _serializer.FromJson(json);
        if (p == null) return;
        
        RuleData = p;
        CompileRules();
        
        _isBuilt = true;
        _isBuilding = false;
    }

    public static IPasswordRuleBuilder New() => new PasswordRuleBuilder();

    internal void AllowWhiteSpace() => RuleData.WhitespaceAllowed = true;
    internal void AllowSpecials() => RuleData.SpecialsAllowed = true;

    internal void AllowNumbers() => RuleData.NumbersAllowed = true;

    internal void MinimumLength(int length)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        if (length < 0)
            throw new PasswordRuleException("Password minimum length cannot be less than 0.");
        
        RuleData.MinimumLengthAllowed = length;
    }

    internal void MinimumLetterCount(int count)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        if (count < 0)
            throw new PasswordRuleException("Password minimum letter count cannot be less than 0.");
        
        RuleData.MinimumLetterCount = count;
    }

    internal void MinimumNumberCount(int count)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        if (count < 0)
            throw new PasswordRuleException("Password minimum number count cannot be less than 0.");

        RuleData.MinimumNumberCount = count;
    }

    internal void MinimumSpecialCount(int count)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        if (count < 0)
            throw new PasswordRuleException("Password minimum letter count cannot be less than 0.");

        RuleData.MinimumSpecialCount = count;
    }

    internal void Expires(DateTime expires)
    {
        if (!_isBuilding)
            throw new PasswordRuleException("Password rules are not being built!");
        if (expires < DateTime.MinValue || expires > DateTime.MaxValue)
            throw new PasswordRuleException("Password expiration was outside of valid range.");

        RuleData.Expiration = expires;
    }

    internal void FinishedBuilding()
    {
        _isBuilding = false;
        _isBuilt = true;

        CompileRules();
    }

    private void CompileRules() => PasswordRules = ToJsonString();

    private string ToJsonString() => _serializer.ToJson(RuleData);
    
    public bool SaveToFile(string filePath) => _serializer.SaveJson(filePath, RuleData);

    public static PasswordRule? ParseFromFile(string filePath)
    {
        var data = new Json<PasswordRuleData>().LoadJson(filePath);
        if (data == null) return null;

        var rule = new PasswordRule(true)
        {
            RuleData = data
        };
        rule.CompileRules();
        return rule;
    }

    public static PasswordRule? Parse(string json) => new(json);

    public string GetValidPassword()
    {
        throw new NotImplementedException();
    }

    public List<IPasswordValidationFailure> CheckAgainstRules(string password)
    {
        return PasswordValidator.Validate(this, password);
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}