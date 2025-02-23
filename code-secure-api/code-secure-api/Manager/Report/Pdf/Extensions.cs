using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace CodeSecure.Manager.Report.Pdf;

public static class Extensions
{
    public static TextBlockDescriptor Heading1(this IContainer container, string? text)
    {
        return container.Text(text).FontSize(24).Bold();
    }

    public static TextBlockDescriptor Heading2(this IContainer container, string? text)
    {
        return container.Text(text).FontSize(20).Bold();
    }
}