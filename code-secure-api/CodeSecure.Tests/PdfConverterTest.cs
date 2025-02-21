using CodeSecure.Extension;

namespace CodeSecure.Tests;

public class PdfConverterTest
{
    [Test]
    public void ConvertHtmlToPdfTest()
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
        var content = File.ReadAllText("../../../../code-secure-api/Resources/Templates/Report/default.html");
        var data = PdfConverter.HtmlToPdf(content);
        File.WriteAllBytes("report.pdf", data);
    }
}