using AetherUtils.Core.Extensions;
using AetherUtils.Core.Security.Passwords;
using AetherUtils.Core.Validation;

namespace AetherUtils.Tests;

public class PasswordRulesTest
{
    private string _passwordOnlyLetters = "Password";
    private string _passwordWithNumbers = "Password123";
    private readonly string passwordWithSpecials = "Password123!";
    private string _passwordWithWhitespace = "Pass Word";
    private readonly string passwordWithAll = "Pass Word123!@";
    private readonly string passphrase = "testPhrase";
    private readonly string encryptedRule = "MukHxacujrGoRG45N5W23rX2z8D54mUwOuPRVWE0AOFReFYEFReeuCnCk1U+MdwWFEJhz" +
                                            "Vv9ji1Y2luxe1z1ED70VypYGXHs97Fw7IjULv+hST/3DCrJvo4mdAt3ymPiQefwyeBbEu" +
                                            "+xfAlN/iaPbtf2AaIXl8VrtNHWFzTbBppWW6oec3S+6u2dUnck5CKbQzm+zvnJic/K8qb" +
                                            "yCoSO3ofTcGzVAxMCRWNz9FRG0bSrJemfFwmSxwb0pG0GOCRuVfN+IiXEvAu+R++uoQcP" +
                                            "M+zXAqXk3X/swj/xKwLmvSEn6g3rxCbeoJCYceP4rnrjFCSTHu7PUB+hnPAFzdlDiw==";

    private const string filePath = "files\\password.rules";
    
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

        var failures = rule.Validate(passwordWithAll);
        Assert.That(failures, Is.Not.Empty);
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }

    [Test]
    public void OnlyLettersTest()
    {
        var rule = PasswordRule.New().Build();
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.Validate(passwordWithAll);
        Assert.That(failures, Is.Not.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }

    [Test]
    public void SpecialsAndNumbersTest()
    {
        var rule = PasswordRule.New().AllowSpecials().AllowNumbers().MinimumLength(passwordWithSpecials.Length).Build();
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.Validate(passwordWithSpecials);
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

        var failures = rule.Validate(passwordWithSpecials);
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
    public async Task TestSaveAndLoadRules()
    {
        var rule = PasswordRule.New().AllowSpecials()
            .AllowNumbers()
            .MinimumNumberCount(1)
            .MinimumSpecialCount(1).Build();
        Console.WriteLine(rule.PasswordRules);
        var saved = await rule.SaveToFileAsync(filePath, passphrase);
        Assert.That(saved, Is.True);
        
        var failures = rule.Validate(passwordWithSpecials);
        Assert.That(failures, Is.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
        
        var rule2 = await PasswordRule.ParseFromFileAsync(filePath, passphrase);
        Assert.That(rule2, Is.Not.Null);
        
        Console.WriteLine(rule2.PasswordRules);
        
        var failures2 = rule2.Validate(passwordWithSpecials);
        Assert.That(failures2, Is.Empty);
        
        failures2.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }
    
    [Test]
    public void ValidPasswordTest()
    {
        var rule = PasswordRule.New().AllowSpecials()
            .AllowNumbers()
            .MinimumNumberCount(1)
            .MinimumSpecialCount(1).Build();
        
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.Validate(passwordWithSpecials);
        Assert.That(failures, Is.Empty);
        
        failures.ForEach(f => Console.WriteLine(((ValidationFailure)f).Message));
    }
    
    [Test]
    public void PasswordRuleEncryptionTest()
    {
        var rule = PasswordRule.New().AllowNumbers().AllowSpecials().AllowWhitespace().MinimumNumberCount(15)
            .MinimumSpecialCount(12).Build();
        Console.WriteLine(rule.PasswordRules);

        var encryptedString = rule.ToEncryptedString(passphrase);
        Console.WriteLine(encryptedString);
        
        Assert.That(encryptedString.IsBase64Encoded(), Is.True);
    }
    
    [Test]
    public void PasswordRuleDecryptionTest()
    {
        var rule = PasswordRule.FromEncryptedString(encryptedRule, passphrase);
        Assert.That(rule, Is.Not.Null);
        
        Console.WriteLine(rule.PasswordRules);

        var failures = rule.Validate(passwordWithAll);
        Assert.That(failures, Has.Count.EqualTo(2));
        failures.ForEach(f => Console.WriteLine(f.Message));
    }

    [Test]
    public void RandomPasswordsTest()
    {
        var rule = PasswordRule.New().AllowSpecials().AllowNumbers().MinimumSpecialCount(3)
            .MinimumNumberCount(3).Build();
        Console.WriteLine(rule.PasswordRules);
        List<string> passwords = [];
        for (int i = 0; i < 100; i++)
        {
            passwords.Add(rule.GetValidPassword());
            Console.WriteLine(passwords[i]);
        }
        
        Assert.That(passwords.TrueForAll(p => rule.Validate(p).Count == 0));
    }

    [Test]
    public void RandomPasswordsWithWhiteSpaceTest()
    {
        PasswordRule rule = PasswordRule.New().AllowSpecials().AllowNumbers().AllowWhitespace().MinimumSpecialCount(5)
            .MinimumNumberCount(4).MinimumLength(15).Build();
        Console.WriteLine(rule.PasswordRules);
        List<string> passwords = [];
        for (int i = 0; i < 100; i++)
        {
            passwords.Add(rule.GetValidPassword());
            Console.WriteLine(passwords[i]);
        }

        foreach (var p in passwords)
        {
            var validation = rule.Validate(p);
            validation.ForEach(v => Console.WriteLine($"{p}: {v.Message}"));
        }
        Assert.That(passwords.TrueForAll(p => rule.Validate(p).Count == 0));
    }
}