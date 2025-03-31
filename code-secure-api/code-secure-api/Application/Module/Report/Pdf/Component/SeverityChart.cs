using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ScottPlot;
using Colors = QuestPDF.Helpers.Colors;

namespace CodeSecure.Application.Module.Report.Pdf.Component;

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
                var slices = new List<PieSlice>();
                if (critical > 0)
                {
                    slices.Add(new PieSlice { Value = critical, FillColor = new ScottPlot.Color(Colors.Red.Medium.Hex), Label = $"Critical - {critical}"});
                }
                if (high > 0)
                {
                    slices.Add(new PieSlice { Value = high, FillColor = new ScottPlot.Color(Colors.Yellow.Medium.Hex), Label = $"High - {high}"});
                }
                if (medium > 0)
                {
                    slices.Add(new PieSlice { Value = medium, FillColor = new ScottPlot.Color(Colors.Yellow.Medium.Hex), Label = $"Medium - {medium}" });
                }
                if (low > 0)
                {
                    slices.Add(new PieSlice { Value = low, FillColor = new ScottPlot.Color(Colors.Lime.Medium.Hex), Label = $"Low - {low}" });
                }
                if (info > 0)
                {
                    slices.Add(new PieSlice { Value = info, FillColor = new ScottPlot.Color(Colors.LightBlue.Medium.Hex), Label = $"Info - {info}" });
                }
                var pie = plot.Add.Pie(slices);
                pie.SliceLabelDistance = 1.2;
                pie.LineWidth = 1;
                pie.LineColor = ScottPlot.Colors.White;
                //
                plot.HideLegend();
                plot.Axes.Frameless();
                plot.HideGrid();
                return plot.GetSvgXml((int)size.Width, (int)size.Height);
            });
        });
        
    }
}