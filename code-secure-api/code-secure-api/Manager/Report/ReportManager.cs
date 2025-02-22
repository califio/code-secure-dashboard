using System.Data;
using System.Text.Json;
using ClosedXML.Excel;
using CodeSecure.Enum;
using CodeSecure.Extension;
using CodeSecure.Manager.Report.Model;
using CodeSecure.Manager.Report.Pdf;
using QuestPDF.Fluent;

namespace CodeSecure.Manager.Report;

public class ReportManager : IReportManager
{
    public byte[] ExportPdf(ReportModel model)
    {
        var document = new ReportDocument(model);
        return document.GeneratePdf();
    }

    public byte[] ExportExcel(ReportModel model)
    {
        using var wb = new XLWorkbook();
        // information
        var information = wb.AddWorksheet("Information");
        information.Cell(1, 1).Value = "Repo Name";
        information.Cell(1, 2).Value = model.RepoName;
        //
        information.Cell(2, 1).Value = "Repo URL";
        information.Cell(2, 2).Value = model.RepoUrl;

        information.Cell(3, 1).Value = "Commit Title";
        information.Cell(3, 2).Value = model.CommitTitle;

        information.Cell(4, 1).Value = "Commit SHA";
        information.Cell(4, 2).Value = model.CommitSha;

        information.Cell(5, 1).Value = "Commit Branch";
        information.Cell(5, 2).Value = model.CommitBranch;

        information.Cell(6, 1).Value = "Scanner";
        information.Cell(6, 2).Value = string.Join(", ", model.Scanners.Select(scanner => scanner.Name).Distinct());

        information.Cell(7, 1).Value = "Last Scan";
        information.Cell(7, 2).Value = model.Time.ToString("dd/MM/yyyy");

        information.Cell(8, 1).Value = "Critical Finding";
        information.Cell(8, 2).Value = model.Findings.Count(finding => finding.Severity == FindingSeverity.Critical);

        information.Cell(9, 1).Value = "High Finding";
        information.Cell(9, 2).Value = model.Findings.Count(finding => finding.Severity == FindingSeverity.High);

        information.Cell(10, 1).Value = "Medium Finding";
        information.Cell(10, 2).Value = model.Findings.Count(finding => finding.Severity == FindingSeverity.Medium);

        information.Cell(11, 1).Value = "Low Finding";
        information.Cell(11, 2).Value = model.Findings.Count(finding => finding.Severity == FindingSeverity.Low);

        information.Cell(12, 1).Value = "Info Finding";
        information.Cell(12, 2).Value = model.Findings.Count(finding => finding.Severity == FindingSeverity.Info);
        // the finding
        var findingDataTable = new DataTable();
        findingDataTable.TableName = "Finding";
        //Add Columns  
        findingDataTable.Columns.Add("ID", typeof(string));
        findingDataTable.Columns.Add("Name", typeof(string));
        findingDataTable.Columns.Add("Severity", typeof(string));
        findingDataTable.Columns.Add("Status", typeof(string));
        findingDataTable.Columns.Add("Location", typeof(string));
        findingDataTable.Columns.Add("Description", typeof(string));
        findingDataTable.Columns.Add("Recommendation", typeof(string));
        findingDataTable.Columns.Add("Ticket", typeof(string));
        //Add Rows in DataTable  
        model.Findings.ForEach(finding =>
        {
            var location = string.Empty;
            if (!string.IsNullOrEmpty(finding.Location))
            {
                location = RepoHelpers.UrlByCommit(model.SourceType, model.RepoUrl, model.CommitSha, finding.Location,
                    finding.StartLine, finding.EndLine);
            }

            findingDataTable.Rows.Add(
                finding.Id,
                finding.Name,
                finding.Severity.ToString().ToUpper(),
                finding.Status.ToString().ToUpper(),
                location,
                finding.Description,
                finding.Recommendation ?? string.Empty,
                finding.TicketUrl
            );
        });
        findingDataTable.AcceptChanges();
        wb.Worksheets.Add(findingDataTable);
        var stream = new MemoryStream();
        wb.SaveAs(stream);
        return stream.GetBuffer();
    }

    public byte[] ExportJson(ReportModel model)
    {
        return JsonSerializer.SerializeToUtf8Bytes(model);
    }
}