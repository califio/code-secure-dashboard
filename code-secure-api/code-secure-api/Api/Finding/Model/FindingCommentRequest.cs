using System.ComponentModel.DataAnnotations;

namespace CodeSecure.Api.Finding.Model;

public class FindingCommentRequest
{
    [Required]
    public required string Comment { get; set; }
}