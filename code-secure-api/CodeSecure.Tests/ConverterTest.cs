using CodeSecure.Extension;

namespace CodeSecure.Tests;

public class ConverterTest
{
    [Test]
    public void MarkdownToJiraTest()
    {
        var input = "Test \n1. step 1\n2.step 2\n\ncode demo\n```java\njava code\n```";
        var output = Converter.MarkdownToJira(input);
        Console.WriteLine(output);
    }
}