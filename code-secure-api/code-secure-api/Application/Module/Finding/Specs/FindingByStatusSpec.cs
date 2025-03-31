using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Finding.Specs;

public class FindingByStatusSpec : BaseSpecification<Findings>
{
    public FindingByStatusSpec(AppDbContext context, params FindingStatus[]? status)
        : base(finding => status == null || status.Length == 0 || context.ScanFindings.Any(record =>
            record.FindingId == finding.Id &&
            status.Contains(record.Status)
        ))
    {
    }

    public FindingByStatusSpec(params FindingStatus[]? status) : base(finding =>
        status == null || status.Length == 0 || status.Contains(finding.Status))
    {
    }
}