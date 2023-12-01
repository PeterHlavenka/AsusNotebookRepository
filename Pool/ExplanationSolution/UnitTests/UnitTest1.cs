using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class Tests
{
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void TestCaseInsensitiveComparer()
    {
        var strings = new string[] { "McHale", "mchale", "MCHALE", "mChale", "mCHale", "MCHale", "mcHale" };
        var currentCultureInfo = System.Globalization.CultureInfo.CurrentCulture;
        var eq1 = strings.Where(s => s.Equals("mchale", StringComparison.CurrentCultureIgnoreCase)); // dokumnetac
        var expected1 = new string[] { "mchale", "MCHALE", "mChale", "mCHale", "MCHale" };
        Assert.That(eq1.SequenceEqual(expected1));

        var eq2 = strings.Where(s => s.Equals("McHale", StringComparison.CurrentCultureIgnoreCase));
        var expected2 = new string[] { "McHale", "mcHale" };
        Assert.That(eq2.SequenceEqual(expected2));
    }
}