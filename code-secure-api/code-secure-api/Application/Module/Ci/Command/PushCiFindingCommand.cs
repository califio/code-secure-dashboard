using CodeSecure.Application.Helpers;
using CodeSecure.Application.Module.Ci.Model;
using CodeSecure.Application.Module.Finding;
using CodeSecure.Application.Module.Integration;
using CodeSecure.Application.Module.Integration.JiraWebhook;
using CodeSecure.Application.Module.Project;
using CodeSecure.Application.Module.Project.Integration;
using CodeSecure.Application.Module.Rule.Command;
using CodeSecure.Application.Module.Rule.Model;
using CodeSecure.Application.Services;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using CodeSecure.Core.Extension;
using CodeSecure.Core.Utils;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Application.Module.Ci.Command;

public class PushCiFindingCommand(IServiceProvider serviceProvider)
{
    private readonly AppDbContext context  = serviceProvider.GetRequiredService<AppDbContext>();
    private readonly ISmtpService smtpService  = serviceProvider.GetRequiredService<ISmtpService>();
    private readonly IRazorRender render  = serviceProvider.GetRequiredService<IRazorRender>();
    private readonly IGlobalAlertManager globalAlertManager  = serviceProvider.GetRequiredService<IGlobalAlertManager>();
    private readonly IJiraWebHookService jiraWebHookService  = serviceProvider.GetRequiredService<IJiraWebHookService>();
    private readonly ILogger<PushCiFindingCommand> logger  = serviceProvider.GetRequiredService<ILogger<PushCiFindingCommand>>();
    
    public async Task<Result<UploadCiFindingResponse>> ExecuteAsync(UploadCiFindingRequest request)
    {
        var scan = await context.Scans
            .Include(scan => scan.Scanner)
            .Include(scan => scan.Commit)
            .FirstOrDefaultAsync(scan => scan.Id == request.ScanId);
        if (scan == null)
        {
            return Result.Fail("Scan not found");
        }

        if (scan.Status != ScanStatus.Running)
        {
            return Result.Fail("Scan not running");
        }
        var project = await context.Projects
            .Include(record => record.SourceControl)
            .FirstAsync(record => record.Id == scan.ProjectId);
        var projectSetting = context.GetProjectSettingsAsync(scan.ProjectId).Result.GetResult();
        foreach (var finding in request.Findings.Where(finding => finding.RuleId != null))
        {
            await new CreateRuleCommand(context).ExecuteAsync(new CreateRuleRequest
            {
                Id = finding.RuleId!,
                ScannerId = scan.ScannerId
            });
        }
        List<FindingStatus> activeStatus = [FindingStatus.Open, FindingStatus.Confirmed, FindingStatus.AcceptedRisk];
        // remove false positive finding & disable finding 
        var falsePositiveFindings = context.Findings
            .Where(finding =>
                finding.ProjectId == scan.ProjectId &&
                finding.ScannerId == scan.ScannerId &&
                finding.Status == FindingStatus.Incorrect
            )
            .Select(finding => finding.Identity).ToHashSet();
        var disabledRules = context.Rules
            .Where(rule => rule.ScannerId == scan.ScannerId && rule.Status == RuleStatus.Disable)
            .Select(rule => rule.Id).ToHashSet();

        var ciFindings = new List<CiFinding>();
        var ciFindingsIdentity = new HashSet<string>();
        request.Findings
            .Where(finding => 
                falsePositiveFindings.Contains(finding.Identity) == false &&
                finding.RuleId != null && disabledRules.Contains(finding.RuleId) == false
            ).ToList().ForEach(finding =>
            {
                ciFindings.Add(finding);
                ciFindingsIdentity.Add(finding.Identity);
            });
        foreach (var finding in ciFindings)
        {
            var existFinding = await context.Findings.FirstOrDefaultAsync(x => x.Identity == finding.Identity && x.ProjectId == scan.ProjectId);
            if (existFinding != null)
            {
                existFinding.Description = finding.Description;
                existFinding.Name = finding.Name;
                existFinding.Location = finding.Location?.Path;
                existFinding.Snippet = finding.Location?.Snippet;
                existFinding.StartLine = finding.Location?.StartLine;
                existFinding.EndLine = finding.Location?.EndLine;
                existFinding.StartColumn = finding.Location?.StartColumn;
                existFinding.EndColumn = finding.Location?.EndColumn;
                context.Findings.Update(existFinding);
                await context.SaveChangesAsync();
            }
        }
        // last scan findings 
        var lastScanFindings = new List<Findings>();
        var lastScanFindingsIdentity = new HashSet<string>();
        context.ScanFindings
            .Include(record => record.Finding)
            .Where(record => record.ScanId == scan.Id && activeStatus.Contains(record.Status))
            .Select(record => record.Finding!).ToList()
            .ForEach(finding =>
            {
                lastScanFindings.Add(finding);
                lastScanFindingsIdentity.Add(finding.Identity);
            });
        var targetBranchFindings = new List<Findings>();
        if (scan.Commit!.Type == CommitType.MergeRequest)
        {
            var targetScanId = context.Scans
                .Where(record =>
                    record.ProjectId == scan.ProjectId
                    && record.ScannerId == scan.ScannerId
                    && record.Commit!.Type == CommitType.CommitBranch
                    && record.Commit.Branch == scan.Commit.TargetBranch)
                .Select(record => record.Id).FirstOrDefault();
            context.ScanFindings
                .Include(record => record.Finding)
                .Where(record => record.ScanId == targetScanId && activeStatus.Contains(record.Status))
                .Select(record => record.Finding!)
                .ToList().ForEach(finding =>
                {
                    targetBranchFindings.Add(finding);
                    lastScanFindingsIdentity.Add(finding.Identity);
                });
        }
        
        // new branch findings 
        var newBranchFindings = ciFindings
            .Where(ciFinding => lastScanFindingsIdentity.Contains(ciFinding.Identity) == false)
            .Select(newFinding => context.CreateFindingAsync(new Findings
            {
                Identity = newFinding.Identity,
                RuleId = newFinding.RuleId,
                Name = newFinding.Name,
                Description = newFinding.Description,
                Recommendation = newFinding.Recommendation,
                Status = FindingStatus.Open,
                Severity = newFinding.Severity,
                Metadata = JSONSerializer.Serialize(newFinding.Metadata),
                ProjectId = scan.ProjectId,
                ScannerId = scan.ScannerId,
                Location = newFinding.Location?.Path,
                Snippet = newFinding.Location?.Snippet,
                StartLine = newFinding.Location?.StartLine,
                EndLine = newFinding.Location?.EndLine,
                StartColumn = newFinding.Location?.StartColumn,
                EndColumn = newFinding.Location?.EndColumn,
                Category = newFinding.Category
            }).Result)
            .Where(x => x.IsSuccess)
            .Select(x => x.Value)
            .ToList();
        // fixed branch findings
        var fixedBranchFindings = lastScanFindings
            .Where(finding => 
                ciFindingsIdentity.Contains(finding.Identity) == false &&
                finding.RuleId != null && disabledRules.Contains(finding.RuleId) == false
            )
            .ToHashSet();
        if (scan.Commit.Type == CommitType.MergeRequest)
        {
            targetBranchFindings.Where(finding => 
                ciFindingsIdentity.Contains(finding.Identity) == false &&
                finding.RuleId != null && disabledRules.Contains(finding.RuleId) == false
            ).ToList().ForEach(finding =>
            {
                fixedBranchFindings.Add(finding);
            });
        }
        if (request.Strategy == ScanStrategy.ChangedFileOnly)
        {
            
            request.ChangedFiles.ForEach(file =>
            {
                if (file.Status == ChangedFileStatus.Delete)
                {
                    logger.LogInformation($"[{file.Status.ToString()}] {file.From}");
                }
                else
                {
                    logger.LogInformation($"[{file.Status.ToString()}] {file.To}");
                }
            });
            var deletedFiles = request.ChangedFiles
                .Where(x => x.Status == ChangedFileStatus.Delete)
                .Select(x => x.From).ToHashSet();
            var addedOrModifiedFiles = request.ChangedFiles
                .Where(x => x.Status != ChangedFileStatus.Delete)
                .Select(x => x.To).ToHashSet();
            fixedBranchFindings = fixedBranchFindings
                .Where(x => deletedFiles.Contains(x.Location) || addedOrModifiedFiles.Contains(x.Location))
                .ToHashSet();
        }
        #region Handle Finding
        #region Handle New Finding
        foreach (var finding in newBranchFindings)
        {
            try
            {
                var newScanFinding = context.ScanFindings.FirstOrDefault(x => x.ScanId == scan.Id && x.FindingId == finding.Id);
                if (newScanFinding == null)
                {
                    newScanFinding = new ScanFindings
                    {
                        ScanId = scan.Id,
                        FindingId = finding.Id,
                        Status = FindingStatus.Open,
                        CommitHash = scan.Commit!.CommitHash ?? string.Empty
                    };
                    context.ScanFindings.Add(newScanFinding);
                    context.FindingActivities.Add(FindingActivities.OpenFinding(finding.Id, scan.CommitId));
                }
                else
                {
                    newScanFinding.CommitHash = scan.Commit!.CommitHash ?? newScanFinding.CommitHash;
                    context.ScanFindings.Update(newScanFinding);
                }
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }
        #endregion
        #region Handle Fixed Finding

        var defaultBranches = projectSetting.GetDefaultBranches();
        foreach (var fixedFinding in fixedBranchFindings)
        {
            // update status (fixed) of finding on this scan
            await context.ScanFindings.Where(record => record.ScanId == scan.Id && record.FindingId == fixedFinding.Id)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(column => column.Status, FindingStatus.Fixed)
                    .SetProperty(column => column.FixedAt, DateTime.UtcNow)
                );
            // add change status activity of finding on scan
            context.FindingActivities.Add(FindingActivities.FixedFinding(fixedFinding.Id, scan.CommitId));
            // fixed on default branch -> fixed finding
            
            if (scan.Commit.IsDefault || defaultBranches.Contains(scan.Commit.Branch))
            {
                fixedFinding.Status = FindingStatus.Fixed;
                fixedFinding.FixedAt = DateTime.UtcNow;
                context.Findings.Update(fixedFinding);
            }
            await context.SaveChangesAsync();
        }
        #endregion
        #region Handle Reopen Finding
        // reopen branch finding: finding fixed in the past but found again in this scan 
        var reopenFindings = context.ScanFindings
            .Include(record => record.Finding)
            .Where(record => record.ScanId == scan.Id && record.Status == FindingStatus.Fixed)
            .Select(record => record.Finding!)
            .ToList()
            .Where(fixedFinding => ciFindingsIdentity.Contains(fixedFinding.Identity))
            .ToList();
        foreach (var finding in reopenFindings)
        {
            await context.ScanFindings.Where(record => record.ScanId == scan.Id && record.FindingId == finding.Id)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.Status, FindingStatus.Confirmed));
            // add reopen activity of finding on scan
            context.FindingActivities.Add(FindingActivities.ReopenFinding(finding.Id, scan.CommitId));
            if (scan.Commit.IsDefault || defaultBranches.Contains(scan.Commit.Branch))
            {
                finding.Status = FindingStatus.Confirmed;
                finding.FixedAt = null;
                context.Findings.Update(finding);
            }
            await context.SaveChangesAsync();
        }
        #endregion
        #endregion
        // the scan findings that found before
        var knownFindings = lastScanFindings
            .Where(finding => fixedBranchFindings.Any(fixedFinding => fixedFinding.Identity == finding.Identity) == false).ToList();
        // update latest commit sha to known finding
        foreach (var finding in knownFindings)
        {
            await context.ScanFindings.Where(record => record.ScanId == scan.Id && record.FindingId == finding.Id)
                .ExecuteUpdateAsync(setter => setter.SetProperty(record => record.CommitHash, scan.Commit.CommitHash));
        }
        // open findings but not verified -> need to verify
        var openFindings = knownFindings.Where(finding => finding.Status == FindingStatus.Open).ToList();
        // confirmed findings but is not fixed -> need to fix
        var confirmedFindings = knownFindings.Where(finding => finding.Status == FindingStatus.Confirmed).ToList();
        
        var response = new UploadCiFindingResponse
        {
            NewFindings = newBranchFindings.Select(CiFinding.FromFinding),
            FixedFindings = fixedBranchFindings.Select(CiFinding.FromFinding),
            ConfirmedFindings = confirmedFindings.Select(CiFinding.FromFinding),
            OpenFindings = openFindings.Select(CiFinding.FromFinding),
            IsBlock = false,
            FindingUrl = $"{FrontendUrlHelper.ProjectFindingUrl(scan.ProjectId)}?commitId={scan.CommitId}&scanner={scan.Scanner?.Name}"
        };
        var isBlock = IsBlock(scan.ProjectId, response, scan.Scanner!.Type);
        response.IsBlock = isBlock;
        // send notification
        var projectAlertManager = new ProjectAlertManager(
            context:context, 
            projectId: project.Id, 
            smtpService: smtpService, 
            render:render);
        if (newBranchFindings.Count != 0)
        {
            var model = new AlertStatusFindingModel
            {
                SourceType = project.SourceControl!.Type,
                Project = project,
                Scanner = scan.Scanner,
                GitCommit = scan.Commit,
                Findings = newBranchFindings,
            };
            _ = globalAlertManager.AlertNewFinding(model);
            _ = projectAlertManager.AlertNewFinding(model);
        }

        if (fixedBranchFindings.Count != 0)
        {
            var model = new AlertStatusFindingModel
            {
                SourceType = project.SourceControl!.Type,
                Project = project,
                Scanner = scan.Scanner,
                GitCommit = scan.Commit,
                Findings = fixedBranchFindings.ToList(),
            };
            _ = globalAlertManager.AlertFixedFinding(model);
            _ = projectAlertManager.AlertFixedFinding(model);
        }

        await jiraWebHookService.AlertScanCompleteAsync(new AlertScanCompleteModel
        {
            SourceType = project.SourceControl!.Type,
            Project = project,
            Scanner = scan.Scanner,
            GitCommit = scan.Commit,
            NewFindingCount = newBranchFindings.Count,
            ConfirmedFindingCount = confirmedFindings.Count,
            FixedFindingCount = fixedBranchFindings.Count,
            IsBlock = isBlock
        });
        return response;
    }
    private bool IsBlock(Guid projectId, UploadCiFindingResponse response, ScannerType scannerType)
    {
        
        var setting = context.GetProjectSettingsAsync(projectId).Result.Value;
        var threshold = scannerType is ScannerType.Dependency or ScannerType.Container ? setting.GetScaSetting() : setting.GetSastSetting();
        if (threshold.Mode == ThresholdMode.MonitorOnly)
        {
            return false;
        }

        List<CiFinding> findings = new();
        if (threshold.Mode == ThresholdMode.BlockOnConfirmation)
        {
            findings.AddRange(response.ConfirmedFindings);
        }
        else
        {
            findings.AddRange(response.NewFindings);
            findings.AddRange(response.OpenFindings);
            findings.AddRange(response.ConfirmedFindings);
        }

        var mSeverities = new Dictionary<FindingSeverity, int>()
        {
            { FindingSeverity.Critical, 0 },
            { FindingSeverity.High, 0 },
            { FindingSeverity.Medium, 0 },
            { FindingSeverity.Low, 0 },
            { FindingSeverity.Info, 0 },
        };
        var now = DateTime.UtcNow;
        foreach (var finding in findings)
        {
            if (finding.FixDeadline == null || finding.FixDeadline < now)
            {
                mSeverities[finding.Severity] += 1;
            }
        }

        if (threshold.Critical > 0 && mSeverities[FindingSeverity.Critical] >= threshold.Critical)
        {
            return true;
        }

        if (threshold.High > 0 && mSeverities[FindingSeverity.High] >= threshold.High)
        {
            return true;
        }

        if (threshold.Medium > 0 && mSeverities[FindingSeverity.Medium] >= threshold.Medium)
        {
            return true;
        }

        if (threshold.Low > 0 && mSeverities[FindingSeverity.Low] >= threshold.Low)
        {
            return true;
        }

        return false;
    }
}