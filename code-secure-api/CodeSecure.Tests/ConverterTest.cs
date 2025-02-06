using CodeSecure.Extension;

namespace CodeSecure.Tests;

public class ConverterTest
{
    [Test]
    public void MarkdownToJiraTest()
    {
        var input = "[dompurify](https://www.npmjs.com/package/dompurify) \r\n```\r\n<div dangerouslySetInnerHTML={{ __html: purify.sanitize(text) }} />\r\n```";
        var output = Converter.MarkdownToJira(input);
        Console.WriteLine(output);
    }
}