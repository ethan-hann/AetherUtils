using AetherUtils.Core.Extensions;

namespace AetherUtils.Tests;

public class NumberExtensionTests
{
    [Test]
    public void FormatSizeTest()
    {
        ulong bytes = 1073741824U;
        ulong pbBytes = 1125899906842624U;
        
        Console.WriteLine(bytes.FormatSize());
        Console.WriteLine(pbBytes.FormatSize());
        
        Assert.That(bytes.FormatSize(), Is.EqualTo("1.00 GB"));
        Assert.That(pbBytes.FormatSize(), Is.EqualTo("1.00 PB"));
    }
}