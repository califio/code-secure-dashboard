using CodeSecure.Application.Exceptions;
using CodeSecure.Application.Module.Ci.Model;
using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Scanner;
using CodeSecure.Application.Module.Scanner.Model;
using CodeSecure.Application.Module.SourceControl;
using CodeSecure.Application.Module.SourceControl.Model;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Ci.Command;

public class CreateCiScanCommand(AppDbContext context)
{
    public async Task<Result<CiScanInfo>> ExecuteAsync(CiScanRequest request)
    {
        Console.WriteLine("Current Commit: " + request.CommitHash);
        if ((request.Type is ScannerType.Sast or ScannerType.Dependency or ScannerType.Secret) == false)
        {
            return Result.Fail($"Not implemented {request.Type.ToString()}");
        }
        // update source control
        var uri = new Uri(request.RepoUrl);
        var sourceUrl = uri.GetLeftPart(UriPartial.Authority);
        var repoPath = uri.AbsolutePath.TrimStart('/');
        var sourceControl = (await context.CreateSourceControlsAsync(new CreateSourceControlRequest
        {
            Url = sourceUrl,
            Type = request.Source
        })).GetResult();
        // update project
        
        var project = await context.CreateProjectAsync(new Projects
        {
            Name = repoPath,
            RepoId = request.RepoId,
            RepoUrl = request.RepoUrl,
            SourceControlId = sourceControl.Id
        });
        // update scanner
        var scanner = (await context.CreateScannerAsync(new CreateScannerRequest
        {
            Name = request.Scanner,
            Type = request.Type,
        })).GetResult();
        // update scan
        var commitQuery = context.ProjectCommits.Where(record =>
            record.ProjectId == project.Id
            && record.Type == request.GitAction
            && record.Branch == request.CommitBranch);
        if (request.GitAction == CommitType.MergeRequest)
        {
            if (string.IsNullOrEmpty(request.TargetBranch)) throw new BadRequestException("Target branch invalid");

            commitQuery = commitQuery.Where(record => record.TargetBranch == request.TargetBranch);
        }

        var commit = await commitQuery.FirstOrDefaultAsync();
        if (commit == null)
        {
            commit = new GitCommits
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                IsDefault = request.IsDefault,
                Branch = request.CommitBranch,
                Type = request.GitAction,
                CommitHash = request.CommitHash,
                CommitTitle = request.ScanTitle,
                TargetBranch = request.TargetBranch,
                MergeRequestId = request.MergeRequestId
            };
            context.ProjectCommits.Add(commit);
            await context.SaveChangesAsync();
        }
        else
        {
            commit.CommitHash = request.CommitHash;
            commit.CommitTitle = request.ScanTitle;
            context.ProjectCommits.Update(commit);
            await context.SaveChangesAsync();
        }
        string? lastCommitSha = null;
        var scan = await context.Scans.FirstOrDefaultAsync(record =>
            record.ProjectId == project.Id &&
            record.ScannerId == scanner.Id &&
            record.CommitId == commit.Id);
        if (scan == null)
        {
            scan = new Scans
            {
                Id = Guid.NewGuid(),
                Status = ScanStatus.Running,
                JobUrl = request.JobUrl,
                StartedAt = DateTime.UtcNow,
                CompletedAt = null,
                Name = request.ScanTitle,
                Metadata = null,
                ProjectId = project.Id,
                ScannerId = scanner.Id,
                CommitId = commit.Id,
                LastCommitHash = request.CommitHash
            };
            context.Scans.Add(scan);
        }
        else
        {
            lastCommitSha = scan.Status != ScanStatus.Completed ? null : scan.LastCommitHash;
            scan.JobUrl = request.JobUrl;
            scan.StartedAt = DateTime.UtcNow;
            scan.CompletedAt = null;
            scan.Status = ScanStatus.Running;
            scan.Name = request.ScanTitle;
            scan.LastCommitHash = request.CommitHash;
            context.Scans.Update(scan);
        }
        await context.SaveChangesAsync();
        Console.WriteLine("Last Commit: " + lastCommitSha);
        return new CiScanInfo
        {
            ScanId = scan.Id,
            ScanUrl = $"{Configuration.FrontendUrl}/#/project/{scan.ProjectId}/overview",
            LastCommitSha = lastCommitSha
        };
    }
}