using CodeSecure.Application.Helpers;
using CodeSecure.Application.Module.Report.Model;
using CodeSecure.Core.Enum;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Markdown;
using Color = QuestPDF.Infrastructure.Color;
using Colors = QuestPDF.Helpers.Colors;

namespace CodeSecure.Application.Module.Report.Pdf;

public class ReportDocument(ReportModel model): IDocument
{
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;
    
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.MarginVertical(30);
            page.MarginHorizontal(50);
            page.PageColor(Colors.BlueGrey.Darken4);
            page.Header().AlignRight().Inlined(line =>
            {
                line.BaselineMiddle();
                line.Spacing(10);
                line.Item().Height(25).Svg(Icons.LogoWhite).FitArea();
                line.Item().Text(Configuration.AppName.ToUpper()).FontColor(Colors.White).FontSize(20).Bold();
            });
            page.Content().AlignMiddle().AlignCenter().Column(col =>
            {
                col.Item().Text("Static Application Security Testing Report").FontSize(48).FontColor(Colors.White).ExtraBlack();
                col.Item().AlignRight().Text($"by {Configuration.AppName}").FontSize(24).FontColor(Colors.White).Bold();
            });
        });
        container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(14));
                page.MarginVertical(15);
                page.MarginHorizontal(50);
                page.Header().Element(Header);
                page.Content().PaddingTop(10).Element(Content);
                page.Footer().Element(Footer);
            });
        
    }

    void Content(IContainer container)
    {
        container.Column(page =>
        {
            page.Item().Element(Information);
            if (model.Findings.Count > 0)
            {
                page.Item().Element(ListFinding);
            }
        });
    }
    
    void Information(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().BorderBottom(1).PaddingBottom(5).BorderColor(Colors.Black).Heading1("Information");
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(130);
                    columns.RelativeColumn();
                });
                table.Cell().Element(CellStyle).Text("Repo Name").Bold();
                table.Cell().Element(CellStyle).Text(model.RepoName);
                // repo url
                table.Cell().Element(CellStyle).Text("Repo URL").Bold();
                table.Cell().Element(CellStyle).Hyperlink(model.RepoUrl).Text(model.RepoUrl);
                // commit title
                table.Cell().Element(CellStyle).Text("Commit Title").Bold();
                table.Cell().Element(CellStyle).Text(model.CommitTitle);
                // commit sha
                table.Cell().Element(CellStyle).Text("Commit SHA").Bold();
                table.Cell().Element(CellStyle)
                    .Hyperlink(GitRepoHelpers.BuildCommitUrl(model.SourceType, model.RepoUrl, model.CommitSha))
                    .Text(model.CommitSha);
                // commit branch
                table.Cell().Element(CellStyle).Text("Commit Branch").Bold();
                table.Cell().Element(CellStyle).Text(model.CommitBranch);
                // merge request
                if (!string.IsNullOrEmpty(model.MergeRequestId))
                {
                    table.Cell().Element(CellStyle).Text("Merge Request").Bold();
                    table.Cell().Element(CellStyle).Text(model.MergeRequestId);
                }
                // scanner
                table.Cell().Element(CellStyle).Text("Scanner").Bold();
                table.Cell().Element(CellStyle).Text(string.Join(", ",model.Scanners.Select(item => item.Name).Distinct()));
                // time
                table.Cell().Element(CellStyle).Text("Last Scan").Bold();
                table.Cell().Element(CellStyle).Text(model.Time.ToString("dd/MM/yyyy"));
                static IContainer CellStyle(IContainer container)
                    => container.Border(1).Padding(8).AlignLeft();
            });
            col.Item().Height(20);
            if (model.Findings.Count > 0)
            {
                /*
                 var critical = model.Findings.Count(finding => finding.Severity == FindingSeverity.Critical);
                   var high = model.Findings.Count(finding => finding.Severity == FindingSeverity.High);
                   var medium = model.Findings.Count(finding => finding.Severity == FindingSeverity.Medium);
                   var low = model.Findings.Count(finding => finding.Severity == FindingSeverity.Low);
                   var info = model.Findings.Count(finding => finding.Severity == FindingSeverity.Info);
                   var open = model.Findings.Count(finding => finding.Status == FindingStatus.Open);
                   var confirmed = model.Findings.Count(finding => finding.Status == FindingStatus.Confirmed);
                   var acceptedRisk = model.Findings.Count(finding => finding.Status == FindingStatus.AcceptedRisk);
                   var resolved = model.Findings.Count(finding => finding.Status == FindingStatus.Fixed);
                   col.Item().MaxWidth(400).AspectRatio(1).Component(new SeverityChart(critical, high, medium, low, info));
                   col.Item().MaxWidth(400).AspectRatio(1).Component(new StatusFindingChart(open, confirmed, acceptedRisk, resolved));
                 */
            }
            else
            {
                col.Item().Text("No Finding").FontSize(18).Bold();
            }
        });
    }

    void ListFinding(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().BorderBottom(1).PaddingBottom(5).BorderColor(Colors.Black).Section("table-finding").Heading1("The Finding");
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(50);
                    columns.RelativeColumn();
                    columns.ConstantColumn(100);
                });
                table.Header(header =>
                {
                    header.Cell().Background(Colors.Indigo.Lighten2).Element(CellStyle).AlignCenter().Text("#").FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Indigo.Lighten2).Element(CellStyle).Text("Vulnerability").FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Indigo.Lighten2).Element(CellStyle).AlignCenter().Text("Severity").FontColor(Colors.White).Bold();
                });
                for (int i = 0; i < model.Findings.Count; i++)
                {
                    var finding = model.Findings[i];
                    table.Cell().Element(CellStyle).AlignCenter().Text($"{i + 1}");
                    table.Cell().Element(CellStyle).SectionLink($"finding-{i}").Text(finding.Name);
                    table.Cell().Background(SeverityColor(finding.Severity)).Element(CellStyle)
                        .AlignCenter()
                        .Text(finding.Severity.ToString().ToUpper());
                }
                static IContainer CellStyle(IContainer container)
                    => container.Border(1).Padding(8).AlignLeft();
            });
            col.Item().Height(20);
            for (var i = 0; i < model.Findings.Count; i++)
            {
                var finding = model.Findings[i];
                var section = $"finding-{i}";
                col.Item().Column(colFinding =>
                {
                    //colFinding.Item().Background(SeverityColor(finding.Severity)).Height(5);
                    colFinding.Item().Section(section).SectionLink("table-finding").Text(finding.Name).FontSize(18).Bold();
                    // severity
                    colFinding.Item().Height(5);
                    colFinding.Item().Table(tb =>
                    {
                        tb.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(75);
                            columns.RelativeColumn();
                        });
                        // severity
                        tb.Cell().Border(1).Background(SeverityColor(finding.Severity)).Padding(5).Text("Severity").Bold();
                        tb.Cell().Border(1).Background(SeverityColor(finding.Severity)).Padding(5).Text(finding.Severity.ToString().ToUpper());
                        // status
                        tb.Cell().Border(1).Padding(5).Text("Status").Bold();
                        tb.Cell().Border(1).Padding(5).Text(finding.Status.ToString().ToUpper());
                        // scanner
                        tb.Cell().Border(1).Padding(5).Text("Scanner").Bold();
                        tb.Cell().Border(1).Padding(5).Text(finding.Scanner);
                        // location
                        if (!string.IsNullOrEmpty(finding.Location))
                        {
                            tb.Cell().Border(1).Padding(5).Text("Location").Bold();
                            var location = finding.Location;
                            if (finding.StartLine != null)
                            {
                                location += $":{finding.StartLine}";
                            }
                            tb.Cell().Border(1).Padding(5)
                                .Hyperlink(GitRepoHelpers.UrlByCommit(model.SourceType, model.RepoUrl, model.CommitSha, finding.Location, finding.StartLine, finding.EndLine))
                                .Text(location);
                            if (!string.IsNullOrEmpty(finding.Snippet))
                            {
                                tb.Cell().Border(1).Padding(5).Text("Snippet").Bold();
                                tb.Cell().Border(1).Padding(5).Text(finding.Snippet.Trim()).FontFamily(Fonts.Consolas);
                            }
                        }
                        // ticket
                        tb.Cell().Border(1).Padding(5).Text("Ticket").Bold();
                        if (finding.TicketUrl != null)
                        {
                            tb.Cell().Border(1).Padding(5).Hyperlink(finding.TicketUrl).Text(finding.TicketName ?? finding.TicketUrl);
                        }
                        else
                        {
                            tb.Cell().Border(1).Padding(5).Text(finding.TicketName);
                        }
                       
                    });
                    //description
                    colFinding.Item().Height(5);
                    colFinding.Item().Markdown(finding.Description);
                    // recommendation
                    if (!string.IsNullOrEmpty(finding.Recommendation))
                    {
                        colFinding.Item().Height(5);
                        colFinding.Item().Text("Recommendation").Bold();
                        colFinding.Item().Markdown(finding.Recommendation);
                    }
                });
                col.Item().PaddingVertical(10).AlignCenter().Width(200).BorderBottom(1);
            }
        });
        
    }

    void Header(IContainer container)
    {
        container.AlignRight().Row(row =>
        {
            row.AutoItem().Height(24).Width(24).Svg(Icons.LogoBlack);
            row.Spacing(5);
            row.AutoItem().Text("CODE SECURE")
                .AlignCenter().FontSize(18).Bold();
        });
    }
    void Footer(IContainer container)
    {
        container.AlignCenter().PaddingTop(5).Text(x =>
        {
            x.Span("Page ");
            x.CurrentPageNumber();
        });
    }
    
    public static Color SeverityColor(FindingSeverity severity)
    {
        if (severity == FindingSeverity.Critical)
        {
            return Colors.Red.Lighten2;
        }

        if (severity == FindingSeverity.High)
        {
            return Colors.Orange.Lighten2;
        }
        if (severity == FindingSeverity.Medium)
        {
            return Colors.Yellow.Lighten2;
        }
        if (severity == FindingSeverity.Low)
        {
            return Colors.LightBlue.Lighten2;
        }
        return Colors.Grey.Lighten2;
    }
}