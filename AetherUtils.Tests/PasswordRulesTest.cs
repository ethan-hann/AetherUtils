using AetherUtils.Core.Security.Passwords;
using AetherUtils.Core.Security.Passwords.Validation;

namespace AetherUtils.Tests;

public class PasswordRulesTest
{
    private string passwordOnlyLetters = "Password";
    private string passwordWithNumbers = "Password123";
    private string passwordWithSpecials = "Password123!";
    private string passwordWithWhitespace = "Pass Word";
    private string passwordWithAll = "Pass Word123!@";
    
    [Test]
    public void BuildRuleTest()
    {
        var rule = PasswordRule.New().AllowSpecials().MinimumLength(5).Build();
        Console.WriteLine(rule.PasswordRules);
        
        Assert.That(rule, Is.Not.Null);
    }

    [Test]
    public void BlankRuleTest()
    {
        var rule = PasswordRule.New().Build();
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.CheckAgainstRules(passwordWithAll);
        Assert.That(failures, Is.Not.Empty);
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }

    [Test]
    public void OnlyLettersTest()
    {
        var rule = PasswordRule.New().MinimumLetterCount(5).Build();
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.CheckAgainstRules(passwordWithAll);
        Assert.That(failures, Is.Not.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }

    [Test]
    public void SpecialsAndNumbersTest()
    {
        var rule = PasswordRule.New().AllowSpecials().AllowNumbers().MinimumLength(passwordWithSpecials.Length).Build();
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.CheckAgainstRules(passwordWithSpecials);
        Assert.That(failures, Is.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }

    [Test]
    public void PasswordExpiredTest()
    {
        var expiration = DateTime.Parse("2023-01-21");
        
        var rule = PasswordRule.New()
            .AllowSpecials()
            .AllowNumbers()
            .MinimumLength(passwordWithSpecials.Length)
            .Expires(expiration).Build();
        
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.CheckAgainstRules(passwordWithSpecials);
        Assert.That(failures, Is.Not.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }

    [Test]
    public void TestParseRules()
    {
        var ruleString = """
                         {
                           "WhitespaceAllowed": false,
                           "SpecialsAllowed": true,
                           "NumbersAllowed": true,
                           "MinimumLengthAllowed": 12,
                           "MinimumLetterCount": -1,
                           "MinimumNumberCount": -1,
                           "MinimumSpecialCount": -1,
                           "Expiration": null
                         }
                         """;
        var rule = PasswordRule.Parse(ruleString);
        Assert.That(rule, Is.Not.Null);
        
        Console.WriteLine(rule.PasswordRules);
    }

    [Test]
    public void TestSaveRules()
    {
        var rule = PasswordRule.New().AllowSpecials()
            .AllowNumbers()
            .MinimumLetterCount(5)
            .MinimumNumberCount(1)
            .MinimumSpecialCount(1).Build();
        Console.WriteLine(rule.PasswordRules);
        var saved = rule.SaveToFile("files\\passwordRules.json");
        Assert.That(saved, Is.True);
        
        var failures = rule.CheckAgainstRules(passwordWithSpecials);
        Assert.That(failures, Is.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }

    [Test]
    public void TestLoadRules()
    {
        var rule = PasswordRule.ParseFromFile("files\\passwordRules.json");
        Assert.That(rule, Is.Not.Null);
        
        Console.WriteLine(rule.PasswordRules);
        
        var failures = rule.CheckAgainstRules(passwordWithSpecials);
        Assert.That(failures, Is.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }

    [Test]
    public void ValidPasswordTest()
    {
        var rule = PasswordRule.New().AllowSpecials()
            .AllowNumbers()
            .MinimumLetterCount(5)
            .MinimumNumberCount(1)
            .MinimumSpecialCount(1).Build();
        
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.CheckAgainstRules(passwordWithSpecials);
        Assert.That(failures, Is.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }
}