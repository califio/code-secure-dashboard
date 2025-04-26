using System.ComponentModel.DataAnnotations;
using CodeSecure.Core.Enum;

namespace CodeSecure.Application.Module.SourceControl.Model;

public record CreateSourceControlRequest
{
    [Required]
    public required string Url { get; set; }
    [Required]
    public required SourceType Type { get; set; }
}