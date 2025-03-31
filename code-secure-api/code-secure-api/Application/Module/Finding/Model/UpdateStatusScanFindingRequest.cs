using System.Text.Json.Serialization;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.Finding.Model;

public class UpdateStatusScanFindingRequest
{
    public Guid ScanId { get; init; }
    public FindingStatus Status { get; init; }
    [JsonIgnore] public Guid CurrentUserId { get; set; }
    [JsonIgnore] public Guid FindingId { get; set; }
}