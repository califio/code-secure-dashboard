using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ScottPlot;
using Color = ScottPlot.Color;
using Colors = QuestPDF.Helpers.Colors;

namespace CodeSecure.Manager.Report.Pdf.Component;

public class StatusFindingChart(int open, int confirmed, int acceptedRisk, int resolved) : IComponent
{
    public void Compose(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Text("Status").FontSize(18).Bold();
            col.Item().Svg(size =>
            {
                Plot plot = new();
                plot.Title("Severity");
                var slices = new PieSlice[]
                {
                    new()
                    {
                        Value = open, FillColor = new Color(Colors.Grey.Medium.Hex), LegendText = $"Open - {open}"
                    },
                    new()
                    {
                        Value = confirmed, FillColor = new Color(Colors.Blue.Medium.Hex), LegendText = $"Confirmed - {confirmed}"
                    },
                    new()
                    {
                        Value = acceptedRisk, FillColor = new Color(Colors.Orange.Medium.Hex),
                        LegendText = $"Accepted Risk - {acceptedRisk}"
                    },
                    new()
                    {
                        Value = resolved, FillColor = new Color(Colors.Green.Medium.Hex), LegendText = $"Fixed - {resolved}"
                    },
                };
                var pie = plot.Add.Pie(slices);
                pie.SliceLabelDistance = 0.7;
                pie.LineWidth = 1;
                pie.LineColor = ScottPlot.Colors.White;
                plot.Axes.Frameless();
                plot.HideGrid();
                return plot.GetSvgXml((int)size.Width, (int)size.Height);
            });
        });
    }
}