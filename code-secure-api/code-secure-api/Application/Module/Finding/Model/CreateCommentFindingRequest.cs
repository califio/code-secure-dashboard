using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Application.Module.Finding.Model;

public record CreateCommentFindingRequest
{
    [Required]
    public required string Comment { get; set; }
}