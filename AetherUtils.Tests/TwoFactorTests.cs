using System.Text;
using AetherUtils.Core.Security.TwoFactor;

namespace AetherUtils.Tests;

public class TwoFactorTests
{
    [Test]
    public void SetupTwoFactorTest()
    {
        var secret = "12345678901234567890123456789012";
        var secretAsByteArray = Encoding.UTF8.GetBytes(secret);
        var issuer = "Test";
        var accountName = "TestAccount";
        var expected = "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZA";
        
        TwoFactorUser u = new TwoFactorUser(issuer, accountName);
        TwoFactorAuth tfa = new TwoFactorAuth();

        tfa.GenerateSetupInformation(ref u, secretAsByteArray);
        Assert.That(u.SetupInformation, Is.Not.Null);
        Assert.That(u.SetupInformation.ManualEntryKey, Is.EqualTo(expected));
    }

    [Test]
    public void SetupWithQrCodeTest()
    {
        var secret = "12345678901234567890123456789012";
        var secretAsByteArray = Encoding.UTF8.GetBytes(secret);
        var issuer = "Test";
        var accountName = "TestAccount";
        var expected = "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZA";
        
        TwoFactorUser u = new TwoFactorUser(issuer, accountName);
        TwoFactorAuth tfa = new TwoFactorAuth();
        
        tfa.GenerateSetupInformation(ref u, secretAsByteArray);
        Assert.That(u.SetupInformation, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(u.SetupInformation.ManualEntryKey, Is.EqualTo(expected));
            Assert.That(u.SetupInformation.QrCodeSetupImageUrl, Is.Not.Empty);
        });
        Console.WriteLine(u.SetupInformation.QrCodeSetupImageUrl);
    }
}