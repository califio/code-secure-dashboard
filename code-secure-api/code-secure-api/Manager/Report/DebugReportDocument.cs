using CodeSecure.Database;
using CodeSecure.Database.Extension;
using CodeSecure.Enum;
using CodeSecure.Manager.Report.Model;
using CodeSecure.Manager.Report.Pdf;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Companion;

namespace CodeSecure.Manager.Report;

public static class DebugReportDocument
{
    public static IApplicationBuilder DebugPdfReport(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        var findings = context.Findings
            .Include(finding => finding.Scanner)
            .Include(finding => finding.Ticket)
            .Page(1, 10).Items.ToList();
        var scanners = context.Scanners.ToList();
        var model = new ReportModel
        {
            SourceType = SourceType.GitLab,
            RepoName = "Project Example",
            RepoUrl = "https://gitlab.com/user01/example",
            CommitTitle = "Commit main branch",
            CommitSha = "1241562626",
            CommitBranch = "main",
            TargetBranch = null,
            MergeRequestId = null,
            Time = DateTime.UtcNow,
            Scanners = scanners.Select(scanner => new ScannerModel
            {
                Name = scanner.Name,
                Type = scanner.Type
            }).ToList(),
            Findings = findings.Select(finding => new FindingModel
            {
                Id = finding.Id,
                Name = finding.Name,
                Description = finding.Description,
                Recommendation = finding.Recommendation,
                Status = finding.Status,
                Severity = finding.Severity,
                Location = finding.Location,
                Snippet = finding.Snippet,
                StartLine = finding.StartLine,
                EndLine = finding.EndLine,
                Scanner = finding.Scanner!.Name,
                Type = finding.Scanner!.Type,
                TicketUrl = finding.Ticket?.Url,
                TicketName = finding.Ticket?.Name
            }).ToList()
        };

        var document = new ReportDocument(model);
        //File.WriteAllBytes("report.pdf", document.GeneratePdf());
        document.ShowInCompanion();
        // var data = new ReportManager().ExportExcel(model);
        // File.WriteAllBytes("report.xlsx", data);
        return app;
    }
}