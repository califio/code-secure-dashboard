using System.Data;
using ClosedXML.Excel;
using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Finding.Model;
using CodeSecure.Authentication.Jwt;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Finding.Command;

public class ExportFindingCommand(AppDbContext context, JwtUserClaims currentUser)
{
    public async Task<Result<byte[]>> ExecuteAsync(FindingFilter filter)
    {
        var query = context.Findings
            .Include(finding => finding.Scanner)
            .Include(finding => finding.Project)
            .Include(finding => finding.Ticket)
            .FindingFilter(context, currentUser, filter)
            .Distinct();
        if (query.Count() > 10000)
        {
            throw new BadRequestException("Total record too large. Max record 10000");
        }

        var findings = await query.OrderBy(record => record.ProjectId).ToListAsync();
        using var wb = new XLWorkbook();
        var findingDt = new DataTable();
        findingDt.TableName = "List Finding";
        //Add Columns  
        findingDt.Columns.Add("Repo", typeof(string));
        findingDt.Columns.Add("Repo URL", typeof(string));
        findingDt.Columns.Add("Scanner", typeof(string));
        findingDt.Columns.Add("Finding", typeof(string));
        findingDt.Columns.Add("Severity", typeof(string));
        findingDt.Columns.Add("Status", typeof(string));
        findingDt.Columns.Add("Location", typeof(string));
        findingDt.Columns.Add("Description", typeof(string));
        findingDt.Columns.Add("Recommendation", typeof(string));
        findingDt.Columns.Add("Ticket", typeof(string));
        //Add Rows in DataTable  
        findings.ForEach(finding =>
        {
            findingDt.Rows.Add(
                finding.Project!.Name,
                finding.Project!.RepoUrl,
                finding.Scanner!.Name,
                finding.Name,
                finding.Severity.ToString().ToUpper(),
                finding.Status.ToString().ToUpper(),
                finding.Location,
                finding.Description,
                finding.Recommendation ?? string.Empty,
                finding.Ticket?.Url ?? string.Empty
            );
        });
        findingDt.AcceptChanges();
        wb.Worksheets.Add(findingDt);
        var stream = new MemoryStream();
        wb.SaveAs(stream);
        return stream.GetBuffer();
    }
}