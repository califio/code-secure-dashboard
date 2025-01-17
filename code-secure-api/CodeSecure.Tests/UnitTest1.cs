using CodeSecure.Extension;

namespace CodeSecure.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestNormalized()
    {
        Console.WriteLine("Test".NormalizeUpper());
        Console.WriteLine("tEst".NormalizeUpper());
        Console.WriteLine(" tEsT ".NormalizeUpper());
    }
    [Test]
    public void TestVersion()
    {
        List<VersionInfo> versions = new();
        var fixedVersion = "1.10.1, 1.2.3-alpha+build.456, 0.5.6, 2.0.10-SNAPHOT, v2.1.1";
        foreach (var version in fixedVersion.Split(","))
        {
            if (VersionInfo.TryParse(version.Trim(), out VersionInfo? v))
            {
                versions.Add(v);
            }
        }
        versions.Sort((v1, v2) => v2.CompareTo(v1));
        foreach (var version in versions)
        {
            Console.WriteLine(version);
        }
    }
}