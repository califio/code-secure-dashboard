using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ScottPlot;
using Colors = QuestPDF.Helpers.Colors;

namespace CodeSecure.Manager.Report.Pdf.Component;

public class SeverityChart(int critical, int high, int medium, int low, int info): IComponent
{
    public void Compose(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Text("Severity").FontSize(18).Bold();
            col.Item().Svg(size =>
            {
                Plot plot = new();
                var slices = new PieSlice[]
                {
                    new() { Value = critical, FillColor = new ScottPlot.Color(Colors.Red.Medium.Hex), LegendText = $"Critical - {critical}"},
                    new() { Value = high, FillColor = new ScottPlot.Color(Colors.Orange.Medium.Hex), LegendText = $"High - {high}"},
                    new() { Value = medium, FillColor = new ScottPlot.Color(Colors.Yellow.Medium.Hex), LegendText = $"Medium - {medium}" },
                    new() { Value = low, FillColor = new ScottPlot.Color(Colors.Lime.Medium.Hex), LegendText = $"Low - {low}" },
                    new() { Value = info, FillColor = new ScottPlot.Color(Colors.LightBlue.Medium.Hex), LegendText = $"Info - {info}" },
                };
                var pie = plot.Add.Pie(slices);
                pie.SliceLabelDistance = 0.7;
                pie.LineWidth = 1;
                pie.LineColor = ScottPlot.Colors.White;
                
                //
                plot.ShowLegend();
                plot.Axes.Frameless();
                plot.HideGrid();
                return plot.GetSvgXml((int)size.Width, (int)size.Height);
            });
        });
        
    }
}